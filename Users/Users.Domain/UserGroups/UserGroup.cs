using Tes.Domain.Contracts;
using Users.Domain.UserGroups.Events;

namespace Users.Domain.UserGroups;

public class UserGroup : AggregateRoot<UserGroupId>
{
    private string _name;
    private string? _description;
    private UserGroupId? _parent;
    private readonly ICollection<RoleId> _roles;

    public UserGroup(string name, RoleId role) : base(UserGroupId.Generate())
    {
        _name = name;
        _roles = new List<RoleId>();
        AssignRole(role);
        AddDomainEvent(new UserGroupCreatedEvent(Id));
    }

    public UserGroup(string name, RoleId role, UserGroupId parent) : this(name, role)
    {
        _parent = parent;
    }

    public static UserGroup From(Name name, RoleId role)
    {
        return new UserGroup(name, role);
    }

    public static UserGroup From(Name name, RoleId role, UserGroupId parent)
    {
        return new UserGroup(name, role, parent);
    }

    public string Name => _name;

    public string? Description => _description;

    public UserGroupId? Parent => _parent;

    public IReadOnlyCollection<RoleId> Roles => (IReadOnlyCollection<RoleId>) _roles;

    public void ChangeName(Name name)
    {
        _name = name;
    }

    public void ChangeDescription(Text description)
    {
        _description = description;
    }

    public void SetParent(UserGroupId parent)
    {
        AddDomainEvent(new UserGroupParentChangedEvent(Id, parent, Parent));
        _parent = parent;
    }

    public void AssignRole(RoleId role)
    {
        _roles.Add(role);
    }

    public void RevokeRole(RoleId role)
    {
        _roles.Remove(role);
    }
}