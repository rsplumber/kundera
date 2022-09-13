using Tes.Domain.Contracts;
using Users.Domain.Users;

namespace Users.Domain;

public class UserGroup : Entity<UserGroupId>
{
    private ICollection<User> Users;
    
    public UserGroup(UserGroupId id) : base(UserGroupId.Generate())
    {
    }
}