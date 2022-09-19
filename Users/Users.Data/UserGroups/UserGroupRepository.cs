using Users.Domain.UserGroups;

namespace Users.Data.UserGroups;

internal class UserGroupRepository : IUserGroupRepository
{
    public Task CreateAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<UserGroup?> FindAsync(UserGroupId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserGroup entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}