using MediatR;
using IAM.API.Domain.Model.Commands;
using IAM.API.Domain.Model.Aggregates;
using IAM.API.Domain.Repositories;
using SharedKernel.Domain.Repositories;

namespace IAM.API.Application.Internal.CommandServices
{
    public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SignUpUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            // Verificar si el email ya existe
            if (await _userRepository.ExistsByEmailAsync(request.Email))
                throw new InvalidOperationException("Email already registered");

            // Hash de la contraseña
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Crear usuario
            var user = new User(
                request.FirstName,
                request.LastName,
                request.Email,
                passwordHash,
                request.Role
            );

            await _userRepository.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return user.Id;
        }
    }
}