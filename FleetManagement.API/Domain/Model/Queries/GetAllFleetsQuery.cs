using SharedKernel.Domain.Model;
using FleetManagement.API.Interfaces.REST.Resources;

// Fleet Queries
namespace FleetManagement.API.Domain.Model.Queries
{
    public record GetAllFleetsQuery() : IQuery<IEnumerable<FleetResource>>;
    
    public record GetFleetByIdQuery(int FleetId) : IQuery<FleetResource?>;
    
    public record GetActiveFleetQuery() : IQuery<IEnumerable<FleetResource>>;
    
    public record GetFleetByTypeQuery(string Type) : IQuery<IEnumerable<FleetResource>>;
    
}