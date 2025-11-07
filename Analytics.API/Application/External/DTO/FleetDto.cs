namespace Analytics.API.Application.External.DTO;

public record FleetDto(
    int Id,
    string Name,
    string Type,
    int ActiveVehicles,
    double Efficiency
);