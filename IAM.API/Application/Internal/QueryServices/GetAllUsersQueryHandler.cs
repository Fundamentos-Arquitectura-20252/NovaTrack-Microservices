using MediatR;
using IAM.API.Domain.Model.Queries;
using IAM.API.Domain.Repositories;
using IAM.API.Interfaces.REST.Resources;
using IAM.API.Interfaces.REST.Transform;


// GetAllUsersQueryHandler.cs
namespace IAM.API.Application.Internal.QueryServices
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResource>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResource>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.ListAsync();
            return users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}