using MediatR;
using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace Flota365.Platform.API.FleetManagement.Application.Internal.CommandServices
{
    public class UpdateFleetCommandHandler : IRequestHandler<UpdateFleetCommand, bool>
    {
        private readonly IFleetRepository _fleetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateFleetCommandHandler(IFleetRepository fleetRepository, IUnitOfWork unitOfWork)
        {
            _fleetRepository = fleetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateFleetCommand request, CancellationToken cancellationToken)
        {
            var fleet = await _fleetRepository.FindByIdAsync(request.FleetId);
            if (fleet == null) return false;

            fleet.UpdateFleetInfo(request.Name, request.Description, request.Type);

            if (request.IsActive)
                fleet.Activate();
            else
                fleet.Deactivate();

            _fleetRepository.Update(fleet);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
