using Personnel.API.Domain.Model.Commands;
using Personnel.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;
using MediatR;

namespace Personnel.API.Application.Internal.CommandServices
{
    public class UpdateDriverStatusCommandHandler : IRequestHandler<UpdateDriverStatusCommand, bool>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDriverStatusCommandHandler(IDriverRepository driverRepository, IUnitOfWork unitOfWork)
        {
            _driverRepository = driverRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateDriverStatusCommand request, CancellationToken cancellationToken)
        {
            var driver = await _driverRepository.FindByIdAsync(request.DriverId);
            if (driver == null) return false;

            switch (request.Status)
            {
                case Domain.Model.Aggregates.DriverStatus.Available:
                    driver.SetAvailable(); break;
                case Domain.Model.Aggregates.DriverStatus.OnRoute:
                    driver.SetOnRoute(); break;
                case Domain.Model.Aggregates.DriverStatus.OnBreak:
                    driver.SetOnBreak(); break;
                case Domain.Model.Aggregates.DriverStatus.Suspended:
                    driver.Suspend("Status updated"); break;
                case Domain.Model.Aggregates.DriverStatus.Inactive:
                    driver.Deactivate(); break;
            }

            _driverRepository.Update(driver);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
