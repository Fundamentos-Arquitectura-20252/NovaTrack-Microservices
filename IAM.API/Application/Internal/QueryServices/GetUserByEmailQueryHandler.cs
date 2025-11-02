using MediatR;
using IAM.API.Domain.Model.Queries;
using IAM.API.Domain.Repositories;
using IAM.API.Interfaces.REST.Resources;
using IAM.API.Interfaces.REST.Transform;



// GetUserByEmailQueryHandler.cs
namespace IAM.API.Application.Internal.QueryServices
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserResource?>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByEmailQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResource?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByEmailAsync(request.Email);
            return user != null ? UserResourceFromEntityAssembler.ToResourceFromEntity(user) : null;
        }
    }
}