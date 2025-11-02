using Microsoft.Extensions.DependencyInjection;
using IAM.API.Domain.Repositories;
using IAM.API.Infrastructure.Persistence.EFC;
using IAM.API.Infrastructure.Persistence.EFC.Repositories;
using SharedKernel.Domain.Repositories;

namespace IAM.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIAMServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}