using Kite.Domain.Contracts;

namespace Domain.UserGroups;

public interface IUserGroupRepository : IRepository<UserGroup, UserGroupId>, IUpdateService<UserGroup>, IDeleteService<UserGroupId>
{
    ValueTask<IEnumerable<UserGroup>> FindAsync(UserGroupId[] groupIds, CancellationToken cancellationToken = default);

    Task<UserGroup?> FindAsync(Name name);
}