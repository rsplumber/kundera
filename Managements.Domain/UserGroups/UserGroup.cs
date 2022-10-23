﻿using Kite.Domain.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.UserGroups.Events;
using Managements.Domain.UserGroups.Exception;
using Managements.Domain.UserGroups.Types;

namespace Managements.Domain.UserGroups;

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

    public static async Task<UserGroup> FromAsync(Name name, RoleId role, IUserGroupRepository userGroupRepository)
    {
        var group = await userGroupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new UserGroupNameDuplicateException();
        }

        return new UserGroup(name, role);
    }

    public static async Task<UserGroup> FromAsync(Name name, RoleId role, UserGroupId parent, IUserGroupRepository userGroupRepository)
    {
        var group = await userGroupRepository.FindAsync(name);
        if (group is not null)
        {
            throw new UserGroupNameDuplicateException();
        }

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
        if (Has(role)) return;

        _roles.Add(role);
        AddDomainEvent(new UserGroupRoleAddedEvent(Id, role));
    }

    public void RevokeRole(RoleId role)
    {
        if (!Has(role)) return;

        if (Roles.Count == 1)
        {
            throw new UserGroupRoleCouldNotBeEmptyException();
        }

        _roles.Remove(role);
        AddDomainEvent(new UserGroupRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return _roles.Any(id => id == role);
    }

    public bool HasParent() => _parent is not null;

    public async Task<IEnumerable<UserGroup>> ParentsAsync(IUserGroupRepository userGroupRepository)
    {
        var groups = new List<UserGroup>();
        groups.Add(this);
        await FetchParentsAsync(this);
        return groups;

        async Task FetchParentsAsync(UserGroup userGroup)
        {
            while (true)
            {
                if (userGroup.HasParent())
                {
                    var org = await userGroupRepository.FindAsync(userGroup.Parent!);
                    if (org is null) continue;
                    userGroup = org;
                    groups.Add(org);
                    continue;
                }

                break;
            }
        }
    }

    public async Task<IEnumerable<Role>> AllWithParentRolesAsync(IUserGroupRepository userGroupRepository, IRoleRepository roleRepository)
    {
        var groups = await ParentsAsync(userGroupRepository);

        var roleIds = groups.SelectMany(group => group.Roles.Select(id => id)).ToArray();

        return await roleRepository.FindAsync(roleIds);
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