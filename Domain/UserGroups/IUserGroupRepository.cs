using Tes.Domain.Contracts;

namespace Domain.UserGroups;

public interface IUserGroupRepository : IRepository<UserGroupId, UserGroup>, IUpdateService<UserGroup>, IDeleteService<UserGroupId>
{
    ValueTask<IEnumerable<UserGroup>> FindAsync(UserGroupId[] groupIds, CancellationToken cancellationToken = default);
    Task<UserGroup?> FindAsync(Name name);
}