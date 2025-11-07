using Analytics.API.Application.External.Services;
using Analytics.API.Infrastructure.Services.External;
using Steeltoe.Discovery.HttpClients;

namespace Analytics.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAnalyticsServices(this IServiceCollection services)
        {
            services.AddHttpClient<IFleetServiceClient, FleetServiceClient>()
                .AddServiceDiscovery();
            services.AddHttpClient<IPersonnelServiceClient, PersonnelServiceClient>()
                .AddServiceDiscovery();
            return services;
        }
    }
}