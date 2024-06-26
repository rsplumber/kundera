﻿using Core.Groups.Events;
using Core.Groups.Exception;
using Core.Roles;

namespace Core.Groups;

public class Group : BaseEntity
{
    public Group()
    {
    }

    internal Group(string name, Role role)
    {
        Name = name;

        Assign(role);

        ChangeStatus(GroupStatus.Enable);
        AddDomainEvent(new GroupCreatedEvent(Id));
    }

    internal Group(string name, Role role, Group parent) : this(name, role)
    {
        Parent = parent;
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public Group? Parent { get; set; }

    public List<Group> Children { get; set; } = new();

    public List<Role> Roles { get; set; } = new();
    
    public GroupStatus Status { get; set; } = default!;

    public DateTime StatusChangeDateUtc { get; set; }

    public void ChangeInfo(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public void SetParent(Group parent)
    {
        AddDomainEvent(new GroupParentChangedEvent(Id, parent.Id, Parent?.Id));
        Parent = parent;
    }

    public void RemoveParent()
    {
        if(!HasAnyParent()) return;
        AddDomainEvent(new GroupParentChangedEvent(Id, null, Parent?.Id));
        Parent = null;
    }

    public void Add(Group child)
    {
        if (HasChild(child)) return;
        Children.Add(child);
        AddDomainEvent(new GroupChildAddedEvent(Id, child.Id));
    }

    public void Remove(Group child)
    {
        if (!HasChild(child)) return;
        Children.Remove(child);
        AddDomainEvent(new GroupChildRemovedEvent(Id, child.Id));
    }


    public void Assign(Role role)
    {
        if (Has(role)) return;
        Roles.Add(role);
        AddDomainEvent(new GroupRoleAddedEvent(Id, role.Id));
    }

    public void Revoke(Role role)
    {
        if (!Has(role)) return;
        if (Roles.Count == 1)
        {
            throw new GroupRoleCouldNotBeEmptyException();
        }

        Roles.Remove(role);
        AddDomainEvent(new GroupRoleRemovedEvent(Id, role.Id));
    }

    public bool Has(Role role)=> Roles.Any(r => r == role);

      public bool HasAnyParent() => Parent is not null;

    public bool HasParent(Group group) => HasAnyParent() && Parent == group;

    public bool HasAnyChild() => Children.Count > 0;

    public bool HasChild(Group child) => HasAnyChild() && Children.Any(g => g == child);

    public void Enable() => ChangeStatus(GroupStatus.Enable);

    public void Disable() => ChangeStatus(GroupStatus.Disable);

    private void ChangeStatus(GroupStatus status)
    {
        Status = status;
        StatusChangeDateUtc = DateTime.UtcNow;
        AddDomainEvent(new GroupStatusChangedEvent(Id, status));
    }
}