using Tes.Domain.Contracts;
using Users.Domain.Users;

namespace Users.Domain.UserGroups;

public class UserGroup : Entity<UserGroupId>
{
    
    public UserGroup(UserGroupId id) : base(UserGroupId.Generate())
    {
    }
}