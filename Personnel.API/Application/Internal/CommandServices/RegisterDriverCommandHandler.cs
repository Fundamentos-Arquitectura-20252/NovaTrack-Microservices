using Personnel.API.Domain.Model.Aggregates;
using Personnel.API.Domain.Model.Commands;
using Personnel.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;
using MediatR;

namespace Personnel.API.Application.Internal.CommandServices
{
    public class RegisterDriverCommandHandler : IRequestHandler<RegisterDriverCommand, int>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterDriverCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
        {
            _driverRepository = driverRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(RegisterDriverCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Code) && await _driverRepository.ExistsByCodeAsync(request.Code))
                throw new InvalidOperationException("Driver code already exists");

            if (await _driverRepository.ExistsByLicenseNumberAsync(request.LicenseNumber))
                throw new InvalidOperationException("License number already exists");

            var driver = new Driver(
                request.Code,
                request.FirstName,
                request.LastName,
                request.LicenseNumber,
                request.LicenseExpiryDate,
                request.Phone,
                request.Email,
                request.ExperienceYears
            );

            await _driverRepository.AddAsync(driver);
            await _unitOfWork.CompleteAsync();

            return driver.Id;
        }
    }
}
