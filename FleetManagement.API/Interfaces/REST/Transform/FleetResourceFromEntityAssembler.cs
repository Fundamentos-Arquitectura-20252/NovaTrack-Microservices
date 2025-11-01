using FleetManagement.API.Domain.Model.Aggregates;
using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Interfaces.REST.Resources;

namespace FleetManagement.API.Interfaces.REST.Transform
{
    public static class FleetResourceFromEntityAssembler
    {
        public static FleetResource ToResourceFromEntity(Fleet entity)
        {
            return new FleetResource(
                entity.Id,
                entity.Code,
                entity.Name,
                entity.Description,
                entity.Type.ToString(),
                entity.IsActive,
                entity.Vehicles.Count,
                entity.GetActiveVehicleCount(),
                entity.Vehicles.Count(v => v.Status == VehicleStatus.Maintenance),
                entity.GetPerformanceRate(),
                entity.CreatedAt,
                entity.UpdatedAt
            );
        }
    }
}
