using Tes.CQRS;
using Users.Application.Users;

namespace Users.Data.Redis.Users;

internal sealed class UsersQueryHandler : QueryHandler<UsersQuery, IEnumerable<UsersResponse>>
{
    public override Task<IEnumerable<UsersResponse>> HandleAsync(UsersQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}