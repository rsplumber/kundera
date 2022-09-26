using Tes.CQRS;
using Users.Application.Users;

namespace Users.Data.Redis.Users;

internal sealed class UserQueryHandler : QueryHandler<UserQuery, UserResponse>
{
    public override Task<UserResponse> HandleAsync(UserQuery message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}