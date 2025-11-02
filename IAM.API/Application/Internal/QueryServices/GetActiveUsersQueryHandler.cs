using MediatR;
using IAM.API.Domain.Model.Queries;
using IAM.API.Domain.Repositories;
using IAM.API.Interfaces.REST.Resources;
using IAM.API.Interfaces.REST.Transform;


// GetActiveUsersQueryHandler.cs
namespace IAM.API.Application.Internal.QueryServices
{
    public class GetActiveUsersQueryHandler : IRequestHandler<GetActiveUsersQuery, IEnumerable<UserResource>>
    {
        private readonly IUserRepository _userRepository;

        public GetActiveUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserResource>> Handle(GetActiveUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindActiveUsersAsync();
            return users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        }
    }
}