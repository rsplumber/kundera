﻿using Domain.Roles;
using Domain.UserGroups.Events;
using Domain.UserGroups.Types;
using Tes.Domain.Contracts;

namespace Domain.UserGroups;

public class UserGroup : AggregateRoot<UserGroupId>
{
    private string _name;
    private string? _description;
    private UserGroupId? _parent;
    private UserGroupStatus _status;
    private DateTime _statusChangedDate;
    private readonly List<RoleId> _roles = new();

    protected UserGroup()
    {
    }

    private UserGroup(string name, RoleId role) : base(UserGroupId.Generate())
    {
        _name = name;

        _roles = new List<RoleId>();
        AssignRole(role);

        ChangeStatus(UserGroupStatus.Enable);
        AddDomainEvent(new UserGroupCreatedEvent(Id));
    }

    private UserGroup(string name, RoleId role, UserGroupId parent) : this(name, role)
    {
        _parent = parent;
    }

    public static UserGroup Create(Name name, RoleId role)
    {
        return new UserGroup(name, role);
    }

    public static UserGroup Create(Name name, RoleId role, UserGroupId parent)
    {
        return new UserGroup(name, role, parent);
    }

    public string Name => _name;

    public string? Description => _description;

    public UserGroupId? Parent => _parent;
    public UserGroupStatus UserGroupStatus => _status;
    public DateTime? StatusChangedDate => _statusChangedDate;

    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

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

    public void RemoveParent()
    {
        AddDomainEvent(new UserGroupParentChangedEvent(Id, null, Parent));
        _parent = null;
    }


    public void AssignRole(RoleId role)
    {
        if (_roles.Contains(role))
        {
            throw new DuplicateRoleNotAssignableException(role.Value);
        }

        _roles.Add(role);
    }

    public void RevokeRole(RoleId role)
    {
        _roles.Remove(role);
    }

    public void Enable() => ChangeStatus(UserGroupStatus.Enable);

    public void Disable() => ChangeStatus(UserGroupStatus.Disable);

    private void ChangeStatus(UserGroupStatus status)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        AddDomainEvent(new UserGroupStatusChangedEvent(Id, status));
    }
}