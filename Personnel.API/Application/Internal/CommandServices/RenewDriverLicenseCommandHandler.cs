using Personnel.API.Domain.Model.Commands;
using Personnel.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;
using MediatR;

namespace Personnel.API.Application.Internal.CommandServices
{
    public class RenewDriverLicenseCommandHandler : IRequestHandler<RenewDriverLicenseCommand, bool>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RenewDriverLicenseCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
        {
            _driverRepository = driverRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RenewDriverLicenseCommand request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.FindByIdAsync(request.DriverId);
            if (driver == null) return false;

            var existingDriver = await _driverRepository.FindByLicenseNumberAsync(request.NewLicenseNumber);
            if (existingDriver != null && existingDriver.Id != request.DriverId)
                throw new InvalidOperationException("New license number already exists");

            driver.UpdateLicense(request.NewLicenseNumber, request.NewExpiryDate);
            _driverRepository.Update(driver);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
