using MediatR;
using IAM.API.Domain.Model.Queries;
using IAM.API.Domain.Repositories;
using IAM.API.Interfaces.REST.Resources;
using IAM.API.Interfaces.REST.Transform;

// GetUserByIdQueryHandler.cs
namespace IAM.API.Application.Internal.QueryServices
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResource?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResource?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByIdAsync(request.UserId);
            return user != null ? UserResourceFromEntityAssembler.ToResourceFromEntity(user) : null;
        }
    }
}