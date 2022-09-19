using Tes.CQRS;
using Users.Application.UserGroups;

namespace Users.Data.UserGroups;

internal sealed class UserGroupQueryHandler : QueryHandler<UserGroupQuery, UserGroupResponse>
{
    public override Task<UserGroupResponse> HandleAsync(UserGroupQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}