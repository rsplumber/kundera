using Tes.Domain.Contracts;

namespace Users.Domain;

public class UserGroup : Entity<UserGroupId>
{
    private ICollection<User> Users;
    
    public UserGroup(UserGroupId id) : base(UserGroupId.Generate())
    {
    }
}