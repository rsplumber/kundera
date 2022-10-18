using Kite.Domain.Contracts;

namespace Managements.Domain.UserGroups;

public interface IUserGroupRepository : IRepository<UserGroup, UserGroupId>, IUpdateService<UserGroup>, IDeleteService<UserGroupId>
{
    Task<IEnumerable<UserGroup>> FindAsync(IEnumerable<UserGroupId> groupIds, CancellationToken cancellationToken = default);

    Task<UserGroup?> FindAsync(Name name, CancellationToken cancellationToken = default);
}