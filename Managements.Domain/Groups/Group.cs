using Kite.Domain.Contracts;
using Managements.Domain.Groups.Events;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Groups.Types;
using Managements.Domain.Roles;

namespace Managements.Domain.Groups;

public class Group : AggregateRoot<GroupId>
{
    protected Group()
    {
    }

    internal Group(Name name, RoleId role) : base(GroupId.Generate())
    {
        Name = name;

        AssignRole(role);

        ChangeStatus(GroupStatus.Enable);
        AddDomainEvent(new GroupCreatedEvent(Id));
    }

    internal Group(Name name, RoleId role, GroupId parent) : this(name, role)
    {
        Parent = parent;
    }


    public Name Name { get; internal set; }

    public Text? Description { get; internal set; }

    public GroupId? Parent { get; internal set; }

    public IReadOnlyCollection<GroupId> Children { get; internal set; } = new List<GroupId>();

    public IReadOnlyCollection<RoleId> Roles { get; internal set; } = new List<RoleId>();

    public GroupStatus Status { get; internal set; }

    public DateTime StatusChangeDate { get; internal set; }

    public void ChangeName(Name name) => Name = name;

    public void ChangeDescription(Text? description) => Description = description;

    public void SetParent(GroupId parent)
    {
        AddDomainEvent(new GroupParentChangedEvent(Id, parent, Parent));
        Parent = parent;
    }

    public void RemoveParent()
    {
        AddDomainEvent(new GroupParentChangedEvent(Id, null, Parent));
        Parent = null;
    }


    public void AssignRole(RoleId role)
    {
        if (Has(role)) return;

        var modifiableRoles = Roles.ToList();
        modifiableRoles.Add(role);
        Roles = modifiableRoles;
        AddDomainEvent(new GroupRoleAddedEvent(Id, role));
    }

    public void RevokeRole(RoleId role)
    {
        if (!Has(role)) return;

        if (Roles.Count == 1)
        {
            throw new GroupRoleCouldNotBeEmptyException();
        }

        var modifiableRoles = Roles.ToList();
        modifiableRoles.Remove(role);
        Roles = modifiableRoles;
        AddDomainEvent(new GroupRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return Roles.Any(id => id == role);
    }

    public bool HasParent() => Parent is not null;

    public bool HasChild() => Children.Count > 0;

    public void Enable() => ChangeStatus(GroupStatus.Enable);

    public void Disable() => ChangeStatus(GroupStatus.Disable);

    private void ChangeStatus(GroupStatus status)
    {
        Status = status;
        StatusChangeDate = DateTime.UtcNow;
        AddDomainEvent(new GroupStatusChangedEvent(Id, status));
    }
}