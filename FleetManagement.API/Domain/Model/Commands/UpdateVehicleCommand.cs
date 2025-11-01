using FleetManagement.API.Domain.Model.Aggregates;
using SharedKernel.Domain.Model;
using FleetManagement.API.Domain.Model.ValueObjects;

namespace FleetManagement.API.Domain.Model.Commands
{
    public record UpdateVehicleCommand(
        int VehicleId,
        string LicensePlate,
        string Brand,
        string Model,
        int Year,
        int Mileage,
        VehicleStatus Status,
        int? FleetId,
        int? DriverId,
        DateTime? LastServiceDate,
        DateTime? NextServiceDate
    ) : ICommand<bool>;
}
