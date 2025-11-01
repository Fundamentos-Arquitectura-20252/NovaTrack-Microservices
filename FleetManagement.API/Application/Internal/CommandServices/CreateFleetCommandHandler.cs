using MediatR;
using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Domain.Model.Aggregates;
using FleetManagement.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace FleetManagement.API.Application.Internal.CommandServices
{
    public class CreateFleetCommandHandler : IRequestHandler<CreateFleetCommand, int>
    {
        private readonly IFleetRepository _fleetRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateFleetCommandHandler(IFleetRepository fleetRepository, IUnitOfWork unitOfWork)
        {
            _fleetRepository = fleetRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateFleetCommand request, CancellationToken cancellationToken)
        {
            if (await _fleetRepository.ExistsByCodeAsync(request.Code))
                throw new InvalidOperationException("Fleet code already exists");

            var fleet = new Fleet(
                request.Code,
                request.Name,
                request.Description,
                request.Type
            );

            await _fleetRepository.AddAsync(fleet);
            await _unitOfWork.CompleteAsync();

            return fleet.Id;
        }
    }
}
