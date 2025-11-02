using Personnel.API.Domain.Repositories;
using Personnel.API.Infrastructure.Persistence.EFC;
using Personnel.API.Infrastructure.Persistence.EFC.Repositories;
using SharedKernel.Domain.Repositories;

namespace Personnel.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMaintenanceServices(this IServiceCollection services)
        {
            // Repositories would be registered here when implemented
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
