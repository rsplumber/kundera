using Core.Domains.Groups.Events;
using Core.Domains.Groups.Exception;

namespace Core.Domains.Groups;

public class Group : BaseEntity
{
    protected Group()
    {
    }

    internal Group(string name, Guid roleId)
    {
        Name = name;

        AssignRole(roleId);

        ChangeStatus(GroupStatus.Enable);
        AddDomainEvent(new GroupCreatedEvent(Id));
    }

    internal Group(string name, Guid roleId, Guid parentId) : this(name, roleId)
    {
        ParentId = parentId;
    }

    public Guid Id { get; internal set; } = Guid.NewGuid();

    public string Name { get; internal set; } = default!;

    public string? Description { get; internal set; }

    public Guid? ParentId { get; internal set; }

    public HashSet<Guid> Children { get; internal set; } = new();

    public HashSet<Guid> Roles { get; internal set; } = new();

    public GroupStatus Status { get; internal set; } = default!;

    public DateTime StatusChangeDateUtc { get; internal set; }

    public void ChangeName(string name) => Name = name;

    public void ChangeDescription(string? description) => Description = description;

    public void SetParent(Guid parent)
    {
        AddDomainEvent(new GroupParentChangedEvent(Id, parent, ParentId));
        ParentId = parent;
    }

    public void RemoveParent()
    {
        AddDomainEvent(new GroupParentChangedEvent(Id, null, ParentId));
        ParentId = null;
    }

    public void AddChild(Guid childId)
    {
        if (HasChild(childId)) return;
        Children.Add(childId);
        AddDomainEvent(new GroupChildAddedEvent(Id, childId));
    }

    public void RemoveChild(Guid childId)
    {
        if (!HasChild(childId)) return;
        Children.Remove(childId);
        AddDomainEvent(new GroupChildRemovedEvent(Id, childId));
    }


    public void AssignRole(Guid role)
    {
        if (HasRole(role)) return;
        Roles.Add(role);
        AddDomainEvent(new GroupRoleAddedEvent(Id, role));
    }

    public void RevokeRole(Guid role)
    {
        if (!HasRole(role)) return;
        if (Roles.Count == 1)
        {
            throw new GroupRoleCouldNotBeEmptyException();
        }

        Roles.Remove(role);
        AddDomainEvent(new GroupRoleRemovedEvent(Id, role));
    }

    public bool HasRole(Guid role)
    {
        return Roles.Any(id => id == role);
    }

    public bool HasParent() => ParentId is not null;

    public bool HasParent(Guid group) => ParentId == group;

    public bool HasChild() => Children.Count > 0;

    public bool HasChild(Guid child) => Children.Any(id => id == child);

    public void Enable() => ChangeStatus(GroupStatus.Enable);

    public void Disable() => ChangeStatus(GroupStatus.Disable);

    private void ChangeStatus(GroupStatus status)
    {
        Status = status;
        StatusChangeDateUtc = DateTime.UtcNow;
        AddDomainEvent(new GroupStatusChangedEvent(Id, status));
    }
}