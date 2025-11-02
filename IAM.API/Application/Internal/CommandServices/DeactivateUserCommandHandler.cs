using MediatR;
using IAM.API.Domain.Model.Commands;
using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;


namespace IAM.API.Application.Internal.CommandServices
{
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByIdAsync(request.UserId);
            if (user == null)
                return false;

            user.Deactivate();
            _userRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}