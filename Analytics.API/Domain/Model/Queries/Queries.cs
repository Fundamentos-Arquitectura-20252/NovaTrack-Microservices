using SharedKernel.Domain.Model;
using Analytics.API.Interfaces.REST.Resources;

namespace Analytics.API.Domain.Model.Queries
{
    public record GetDashboardStatsQuery() : IQuery<DashboardStatsResource>;
    
    public record GetFleetSummaryQuery() : IQuery<FleetSummaryResource>;
    
    public record GetActiveVehiclesQuery() : IQuery<IEnumerable<ActiveVehicleResource>>;
}