using Microsoft.Extensions.DependencyInjection;
using FleetManagement.API.Domain.Repositories;
using FleetManagement.API.Infrastructure.Persistence.EFC;
using FleetManagement.API.Infrastructure.Persistence.EFC.Repositories;
using SharedKernel.Domain.Repositories;

namespace FleetManagement.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFleetManagementServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IFleetRepository, FleetRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}