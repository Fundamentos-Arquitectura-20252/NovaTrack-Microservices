using System.Threading; 
using Microsoft.Extensions.Primitives;
using Steeltoe.Common.Discovery;
using Yarp.ReverseProxy.Configuration;

public class EurekaProxyConfigProvider : IProxyConfigProvider
{
    private readonly IDiscoveryClient _discoveryClient;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(30);
    private readonly object _sync = new();
    private IProxyConfig _config;
    private CancellationTokenSource _currentCts = new();
        
    public EurekaProxyConfigProvider(IDiscoveryClient discoveryClient)
    {
        Console.WriteLine(">>> Provider CREATED!");
        _discoveryClient = discoveryClient;
        // Inicializamos config
        _config = BuildConfigAsync(_currentCts.Token).GetAwaiter().GetResult();
        StartRefreshLoop();
        
        // Log para verificar qué rutas detectó
        Console.WriteLine(">>> EUREKA ROUTES DETECTED:");
        foreach (var r in _config.Routes)
            Console.WriteLine($" - ID: {r.RouteId} | Path: {r.Match.Path}");
    }
        
    public IProxyConfig GetConfig()
    {
        lock (_sync) return _config;
    }
        
    private void StartRefreshLoop()
    {
        Task.Run(async () =>
        {
            while (!_currentCts.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_refreshInterval, _currentCts.Token);
                    var newConfig = await BuildConfigAsync(_currentCts.Token);
        
                    lock (_sync)
                    {
                        bool different = newConfig.Routes.Count != _config.Routes.Count || newConfig.Clusters.Count != _config.Clusters.Count;
                        if (!different)
                        {
                            var oldRouteIds = _config.Routes.Select(r => r.RouteId).OrderBy(x => x);
                            var newRouteIds = newConfig.Routes.Select(r => r.RouteId).OrderBy(x => x);
                            different = !oldRouteIds.SequenceEqual(newRouteIds);
                        }
        
                        if (different)
                        {
                            _currentCts.Cancel();
                            _currentCts = new CancellationTokenSource();
                            _config = new InMemoryConfig(newConfig.Routes, newConfig.Clusters, _currentCts.Token);
                        }
                    }
                }
                catch (OperationCanceledException) { break; }
                catch { }
            }
        });
    }
        
    private async Task<InMemoryConfig> BuildConfigAsync(CancellationToken cancellationToken)
    {
        var routes = new List<RouteConfig>();
        var clusters = new List<ClusterConfig>();

        var rawServiceIds = await _discoveryClient.GetServiceIdsAsync(cancellationToken);

        // VOLVEMOS A TU LÓGICA ORIGINAL:
        // 1. Normalizamos los IDs primero
        var normalizedIds = rawServiceIds
            .Select(id => id.ToLowerInvariant().Replace(".api", "").Replace(".", ""))
            .Distinct()
            .ToList();

        // 2. Iteramos sobre los IDs YA NORMALIZADOS (como lo tenías antes)
        foreach (var normalizedId in normalizedIds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // 3. Pedimos a Eureka usando el ID NORMALIZADO (Esto es lo que te funcionaba)
            var instances = await _discoveryClient.GetInstancesAsync(normalizedId, cancellationToken);

            if (!instances.Any()) continue;

            var destinations = instances
                .Select((instance, index) =>
                {
                    var scheme = instance.IsSecure ? "https" : "http";
                    var address = $"{scheme}://{instance.Host}:{instance.Port}";
                    return new KeyValuePair<string, DestinationConfig>(
                        $"dest-{index}",
                        new DestinationConfig { Address = address });
                })
                .ToDictionary(k => k.Key, v => v.Value);

            clusters.Add(new ClusterConfig
            {
                ClusterId = normalizedId,
                Destinations = destinations
            });

            // Agregamos la ruta con la Transformación necesaria para el Gateway
            routes.Add(new RouteConfig
            {
                RouteId = normalizedId,
                ClusterId = normalizedId,
                Match = new RouteMatch
                {
                    Path = $"/{normalizedId}/{{**catch-all}}"
                },
                // ESTO ES LO NUEVO QUE NECESITAS PARA SWAGGER/YARP:
                // Sin esto, la ruta llega duplicada al microservicio.
                Transforms = new List<IReadOnlyDictionary<string, string>>
                {
                    new Dictionary<string, string>
                    {
                        { "PathRemovePrefix", $"/{normalizedId}" }
                    }
                }
            });
        }

        return new InMemoryConfig(routes, clusters, cancellationToken);
    }

    private class InMemoryConfig : IProxyConfig
    {
        public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters, CancellationToken token)
        {
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(token);
        }
        
        public IReadOnlyList<RouteConfig> Routes { get; }
        public IReadOnlyList<ClusterConfig> Clusters { get; }
        public IChangeToken ChangeToken { get; }
    }
}