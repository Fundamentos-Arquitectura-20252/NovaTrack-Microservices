using MediatR;
using FleetManagement.API.Domain.Model.Commands;
using FleetManagement.API.Domain.Model.Aggregates;
using FleetManagement.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace FleetManagement.API.Application.Internal.CommandServices
{
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, int>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
        {
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            if (await _vehicleRepository.ExistsByLicensePlateAsync(request.LicensePlate))
                throw new InvalidOperationException("License plate already exists");

            var vehicle = new Vehicle(
                request.LicensePlate,
                request.Brand,
                request.Model,
                request.Year,
                request.Mileage,
                request.FleetId,
                request.DriverId
            );

            await _vehicleRepository.AddAsync(vehicle);
            await _unitOfWork.CompleteAsync();

            return vehicle.Id;
        }
    }
}
