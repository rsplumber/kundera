using Tes.Domain.Contracts;

namespace Users.Domain.UserGroups;

public interface IUserGroupRepository : IRepository<UserGroupId, UserGroup>, IUpdateService<UserGroup>
{
}