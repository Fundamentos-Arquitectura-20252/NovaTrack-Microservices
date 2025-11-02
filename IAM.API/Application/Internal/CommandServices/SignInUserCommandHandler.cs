using MediatR;
using IAM.API.Domain.Model.Commands;
using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace IAM.API.Application.Internal.CommandServices
{
    public class SignInUserCommandHandler : IRequestHandler<SignInUserCommand, int>
    {
        private readonly IUserRepository _userRepository;

        public SignInUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            
            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            return user.Id;
        }
    }
}
