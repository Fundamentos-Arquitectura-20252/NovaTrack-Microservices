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
        _config = BuildConfigAsync(_currentCts.Token).GetAwaiter().GetResult();
        StartRefreshLoop();
        Console.WriteLine(">>> EUREKA ROUTES:");
        foreach (var r in _config.Routes)
            Console.WriteLine(" - " + r.RouteId + " | " + r.Match.Path);

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
        
                    // Replace config and signal change token if different
                    lock (_sync)
                    {
                        // crude equality check: different counts or cluster ids -> update
                        bool different = newConfig.Routes.Count != _config.Routes.Count || newConfig.Clusters.Count != _config.Clusters.Count;
        
                        if (!different)
                        {
                            // further check ids
                            var oldRouteIds = _config.Routes.Select(r => r.RouteId).OrderBy(x => x);
                            var newRouteIds = newConfig.Routes.Select(r => r.RouteId).OrderBy(x => x);
                            different = !oldRouteIds.SequenceEqual(newRouteIds);
                        }
        
                        if (different)
                        {
                            // cancel previous token so YARP reloads
                            _currentCts.Cancel();
                            _currentCts = new CancellationTokenSource();
        
                            // wrap new routes/clusters into InMemoryConfig with new token
                            _config = new InMemoryConfig(newConfig.Routes, newConfig.Clusters, _currentCts.Token);
                        }
                    }
                }
                catch (OperationCanceledException) { break; }
                catch
                {
                    // swallow – will retry on next loop; consider logging
                }
            }
        });
    }
        
    private async Task<InMemoryConfig> BuildConfigAsync(CancellationToken cancellationToken)
    {
        var routes = new List<RouteConfig>();
        var clusters = new List<ClusterConfig>();

        // Normalizamos y deduplicamos serviceIds
        var rawServiceIds = await _discoveryClient.GetServiceIdsAsync(cancellationToken);

        var normalizedIds = rawServiceIds
            .Select(id => id.ToLowerInvariant().Replace(".api", "").Replace(".", ""))
            .Distinct()
            .ToList();

        foreach (var normalizedId in normalizedIds)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Obtener instancias usando el serviceId REAL original
            var instances = await _discoveryClient.GetInstancesAsync(normalizedId, cancellationToken);

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

            routes.Add(new RouteConfig
            {
                RouteId = normalizedId,
                ClusterId = normalizedId,
                Match = new RouteMatch
                {
                    Path = $"/{normalizedId}/{{**catch-all}}"
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