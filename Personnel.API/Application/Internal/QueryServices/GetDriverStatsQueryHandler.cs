using Personnel.API.Domain.Model.Aggregates;
using MediatR;
using Personnel.API.Domain.Model.Queries;
using Personnel.API.Domain.Repositories;
using Personnel.API.Interfaces.REST.Resources;
using Personnel.API.Domain.Model.ValueObjects;

namespace Personnel.API.Application.Internal.QueryServices
{
    public class GetDriverStatsQueryHandler : IRequestHandler<GetDriverStatsQuery, DriverStatsResource>
    {
        private readonly IDriverRepository _driverRepository;

        public GetDriverStatsQueryHandler(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<DriverStatsResource> Handle(GetDriverStatsQuery request, CancellationToken cancellationToken)
        {
            var allDrivers = await _driverRepository.ListAsync();
            var activeDrivers = await _driverRepository.FindActiveDriversAsync();
            var availableDrivers = await _driverRepository.FindAvailableDriversAsync();
            var expiredLicenses = await _driverRepository.FindDriversWithExpiredLicensesAsync();
            var expiringSoon = await _driverRepository.FindDriversWithExpiringSoonLicensesAsync();

            var totalDrivers = allDrivers.Count();
            var activeCount = activeDrivers.Count();
            var inactiveCount = totalDrivers - activeCount;

            var driversByStatus = allDrivers
                .GroupBy(d => d.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            var driversByExperience = allDrivers
                .GroupBy(d => new ExperienceLevel(d.ExperienceYears).Level)
                .ToDictionary(g => g.Key, g => g.Count());

            return new DriverStatsResource(
                TotalDrivers: totalDrivers,
                ActiveDrivers: activeCount,
                InactiveDrivers: inactiveCount,
                AvailableDrivers: availableDrivers.Count(),
                DriversOnRoute: allDrivers.Count(d => d.Status == DriverStatus.OnRoute),
                DriversOnBreak: allDrivers.Count(d => d.Status == DriverStatus.OnBreak),
                SuspendedDrivers: allDrivers.Count(d => d.Status == DriverStatus.Suspended),
                ExpiredLicenses: expiredLicenses.Count(),
                LicensesExpiringSoon: expiringSoon.Count(),
                AverageExperience: totalDrivers > 0 ? Math.Round(allDrivers.Average(d => d.ExperienceYears), 1) : 0,
                ExperiencedDrivers: allDrivers.Count(d => d.ExperienceYears >= 5),
                SeniorDrivers: allDrivers.Count(d => d.ExperienceYears >= 10),
                DriversByExperienceLevel: driversByExperience,
                DriversByStatus: driversByStatus
            );
        }
    }
}