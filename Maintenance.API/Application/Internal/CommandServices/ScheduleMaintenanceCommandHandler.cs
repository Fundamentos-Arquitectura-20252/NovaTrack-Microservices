using MediatR;
using Maintenance.API.Domain.Model.Commands;
using Maintenance.API.Domain.Model.Aggregates;
using Maintenance.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace Maintenance.API.Application.Internal.CommandServices
{
    public class ScheduleMaintenanceCommandHandler : IRequestHandler<ScheduleMaintenanceCommand, int>
    {
        private readonly IMaintenanceRecordRepository _maintenanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleMaintenanceCommandHandler(IMaintenanceRecordRepository maintenanceRepository, IUnitOfWork unitOfWork)
        {
            _maintenanceRepository = maintenanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(ScheduleMaintenanceCommand request, CancellationToken cancellationToken)
        {
            var existingMaintenance = await _maintenanceRepository.FindPendingByVehicleAndTypeAsync(request.VehicleId, request.Type);
            if (existingMaintenance != null)
                throw new InvalidOperationException($"Vehicle already has pending {request.Type} maintenance scheduled");

            var maintenance = new MaintenanceRecord(
                request.VehicleId,
                request.Description,
                request.Type,
                request.EstimatedCost,
                request.ScheduledDate,
                request.ServiceProvider
            );

            await _maintenanceRepository.AddAsync(maintenance);
            await _unitOfWork.CompleteAsync();

            return maintenance.Id;
        }
    }
}
