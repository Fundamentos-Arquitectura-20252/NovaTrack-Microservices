using FleetManagement.API.Domain.Model.Aggregates;
using FleetManagement.API.Interfaces.REST.Resources;

namespace FleetManagement.API.Interfaces.REST.Transform
{
    public static class VehicleResourceFromEntityAssembler
    {
        public static VehicleResource ToResourceFromEntity(Vehicle entity)
        {
            return new VehicleResource(
                entity.Id,
                entity.LicensePlate.Value,
                entity.Brand,
                entity.Model,
                entity.Year,
                entity.Mileage,
                entity.Status.ToString(),
                entity.FleetId,
                entity.Fleet?.Name,
                entity.DriverId,
                null, // DriverName - se llenará desde Personnel context
                entity.LastServiceDate,
                entity.NextServiceDate,
                entity.IsServiceDue(),
                entity.CreatedAt,
                entity.UpdatedAt
            );
        }
    }
}
