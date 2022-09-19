using Tes.CQRS;
using Users.Application.UserGroups;

namespace Users.Data.UserGroups;

internal sealed class UserGroupsQueryHandler : QueryHandler<UserGroupsQuery, IEnumerable<UserGroupsResponse>>
{
    public override Task<IEnumerable<UserGroupsResponse>> HandleAsync(UserGroupsQuery message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}