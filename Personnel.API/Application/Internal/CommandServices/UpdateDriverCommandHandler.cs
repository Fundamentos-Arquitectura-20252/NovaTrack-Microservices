using Personnel.API.Domain.Model.Commands;
using Personnel.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;
using MediatR;

namespace Personnel.API.Application.Internal.CommandServices
{
    public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, bool>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
        {
            _driverRepository = driverRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.FindByIdAsync(request.DriverId);
            if (driver == null) return false;

            var existingDriver = await _driverRepository.FindByLicenseNumberAsync(request.LicenseNumber);
            if (existingDriver != null && existingDriver.Id != request.DriverId)
                throw new InvalidOperationException("License number already exists");

            driver.UpdatePersonalInfo(request.FirstName, request.LastName, request.ExperienceYears);
            driver.UpdateContactInfo(request.Phone, request.Email);
            driver.UpdateLicense(request.LicenseNumber, request.LicenseExpiryDate);

            switch (request.Status)
            {
                case Domain.Model.Aggregates.DriverStatus.Available:
                    driver.SetAvailable(); break;
                case Domain.Model.Aggregates.DriverStatus.OnRoute:
                    driver.SetOnRoute(); break;
                case Domain.Model.Aggregates.DriverStatus.OnBreak:
                    driver.SetOnBreak(); break;
                case Domain.Model.Aggregates.DriverStatus.Suspended:
                    driver.Suspend("Updated via command"); break;
                case Domain.Model.Aggregates.DriverStatus.Inactive:
                    driver.Deactivate(); break;
            }

            _driverRepository.Update(driver);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
