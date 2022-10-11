using Tes.Domain.Contracts;

namespace Domain.UserGroups;

public interface IUserGroupRepository : IRepository<UserGroupId, UserGroup>, IUpdateService<UserGroup>, IDeleteService<UserGroupId>
{
    Task<UserGroup?> FindAsync(Name name);
}