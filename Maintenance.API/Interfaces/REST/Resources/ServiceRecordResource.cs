namespace Maintenance.API.Interfaces.REST.Resources
{
    public record ServiceRecordResource(
        int Id,
        int VehicleId,
        string VehicleLicensePlate,
        string ServiceType,
        string Description,
        decimal Cost,
        string Currency,
        DateTime ServiceDate,
        int MileageAtService,
        string ServiceProvider,
        string TechnicianName,
        string Quality,
        string PartsUsed,
        string Notes,
        DateTime? NextServiceDue,
        int? NextServiceMileage,
        bool IsWarrantyValid,
        bool IsRecentService,
        DateTime CreatedAt
    );
}
