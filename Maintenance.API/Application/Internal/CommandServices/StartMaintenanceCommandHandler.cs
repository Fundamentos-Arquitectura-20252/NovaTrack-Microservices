using MediatR;
using Maintenance.API.Domain.Model.Commands;
using Maintenance.API.Domain.Model.Aggregates;
using Maintenance.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace Maintenance.API.Application.Internal.CommandServices
{
    public class StartMaintenanceCommandHandler : IRequestHandler<StartMaintenanceCommand, bool>
    {
        private readonly IMaintenanceRecordRepository _maintenanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StartMaintenanceCommandHandler(IMaintenanceRecordRepository maintenanceRepository, IUnitOfWork unitOfWork)
        {
            _maintenanceRepository = maintenanceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(StartMaintenanceCommand request, CancellationToken cancellationToken)
        {
            var maintenance = await _maintenanceRepository.FindByIdAsync(request.MaintenanceId);
            if (maintenance == null) return false;

            maintenance.StartMaintenance();
            _maintenanceRepository.Update(maintenance);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
