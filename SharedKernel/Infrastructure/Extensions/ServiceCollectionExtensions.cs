using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using SharedKernel.Domain.Model.Events;
using SharedKernel.Domain.Repositories;
using SharedKernel.Infrastructure.Configuration;

namespace SharedKernel.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            services.Configure<ApplicationSettings>(configuration => { });
            services.Configure<DatabaseSettings>(configuration => { });
            services.Configure<CachingSettings>(configuration => { });
            services.Configure<LoggingSettings>(configuration => { });

            return services;
        }

        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApplicationSettings>(configuration.GetSection("Application"));
            services.Configure<DatabaseSettings>(configuration.GetSection("Database"));
            services.Configure<CachingSettings>(configuration.GetSection("Caching"));
            services.Configure<LoggingSettings>(configuration.GetSection("Logging"));

            services.AddSharedServices();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}