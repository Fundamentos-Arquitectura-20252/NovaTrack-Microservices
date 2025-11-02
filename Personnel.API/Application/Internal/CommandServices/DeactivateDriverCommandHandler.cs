using Personnel.API.Domain.Model.Commands;
using Personnel.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;
using MediatR;

namespace Personnel.API.Application.Internal.CommandServices
{
    public class DeactivateDriverCommandHandler : IRequestHandler<DeactivateDriverCommand, bool>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
        {
            _driverRepository = driverRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeactivateDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.FindByIdAsync(request.DriverId);
            if (driver == null) return false;

            var hasAssignedVehicles = await _driverRepository.HasAssignedVehiclesAsync(request.DriverId);
            if (hasAssignedVehicles)
                throw new InvalidOperationException("Cannot deactivate driver with assigned vehicles");

            driver.Deactivate();
            _driverRepository.Update(driver);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
