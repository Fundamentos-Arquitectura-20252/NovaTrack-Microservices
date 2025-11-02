using MediatR;
using Maintenance.API.Domain.Model.Commands;
using Maintenance.API.Domain.Model.Aggregates;
using Maintenance.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace Maintenance.API.Application.Internal.CommandServices
{
    public class CreateServiceRecordCommandHandler : IRequestHandler<CreateServiceRecordCommand, int>
    {
        private readonly IServiceRecordRepository _serviceRecordRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateServiceRecordCommandHandler(IServiceRecordRepository serviceRecordRepository, IUnitOfWork unitOfWork)
        {
            _serviceRecordRepository = serviceRecordRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateServiceRecordCommand request, CancellationToken cancellationToken)
        {
            var serviceRecord = new ServiceRecord(
                request.VehicleId,
                request.ServiceType,
                request.Description,
                request.Cost,
                request.ServiceDate,
                request.MileageAtService,
                request.ServiceProvider,
                request.TechnicianName
            );

            if (!string.IsNullOrEmpty(request.PartsUsed))
                serviceRecord.AddPartsUsed(request.PartsUsed);

            if (!string.IsNullOrEmpty(request.Notes))
                serviceRecord.AddNotes(request.Notes);

            await _serviceRecordRepository.AddAsync(serviceRecord);
            await _unitOfWork.CompleteAsync();

            return serviceRecord.Id;
        }
    }
}
