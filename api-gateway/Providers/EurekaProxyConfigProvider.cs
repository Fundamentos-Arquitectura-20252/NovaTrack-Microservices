using Microsoft.Extensions.Primitives;
using Steeltoe.Common.Discovery;
using Yarp.ReverseProxy.Configuration;

public class EurekaProxyConfigProvider : IProxyConfigProvider
{
    private readonly IDiscoveryClient _discoveryClient;
    private readonly CancellationTokenSource _cts = new();
    private readonly IProxyConfig _config;

    public EurekaProxyConfigProvider(IDiscoveryClient discoveryClient)
    {
        _discoveryClient = discoveryClient;
        _config = BuildConfigAsync().GetAwaiter().GetResult();
    }

    public IProxyConfig GetConfig() => _config;

    private async Task<IProxyConfig> BuildConfigAsync()
    {
        var routes = new List<RouteConfig>();
        var clusters = new List<ClusterConfig>();

        var serviceIds = await _discoveryClient.GetServiceIdsAsync(CancellationToken.None);

        foreach (var serviceId in serviceIds)
        {
            var instances = await _discoveryClient.GetInstancesAsync(serviceId, CancellationToken.None);

            var destinations = instances
                .Select((instance, index) => new KeyValuePair<string, DestinationConfig>(
                    $"dest-{index}",
                    new DestinationConfig
                    {
                        Address = $"http://{instance.Host}:{instance.Port}"
                    }))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            clusters.Add(new ClusterConfig
            {
                ClusterId = serviceId.ToLower(),
                Destinations = destinations
            });

            routes.Add(new RouteConfig
            {
                RouteId = serviceId.ToLower(),
                ClusterId = serviceId.ToLower(),
                Match = new RouteMatch
                {
                    Path = $"/{serviceId.ToLower()}/{{**catch-all}}"
                }
            });
        }

        return new InMemoryConfig(routes, clusters);
    }

    private class InMemoryConfig : IProxyConfig
    {
        public InMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(new CancellationToken());
        }

        public IReadOnlyList<RouteConfig> Routes { get; }
        public IReadOnlyList<ClusterConfig> Clusters { get; }
        public IChangeToken ChangeToken { get; }
    }
}