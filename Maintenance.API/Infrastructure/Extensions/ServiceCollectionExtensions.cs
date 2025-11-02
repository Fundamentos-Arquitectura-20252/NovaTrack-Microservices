using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Maintenance.API.Domain.Model.Commands;
using Maintenance.API.Domain.Model.Queries;
using Maintenance.API.Interfaces.REST.Resources;
using Maintenance.API.Domain.Repositories;
using Maintenance.API.Infrastructure.Persistence.EFC;
using Maintenance.API.Infrastructure.Persistence.EFC.Repositories;
using SharedKernel.Domain.Repositories;


// ServiceCollectionExtensions.cs
namespace Maintenance.API.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMaintenanceServices(this IServiceCollection services)
        {
            // Repositories would be registered here when implemented
             services.AddScoped<IMaintenanceRecordRepository, MaintenanceRecordRepository>();
             services.AddScoped<IServiceRecordRepository, ServiceRecordRepository>();
             services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}