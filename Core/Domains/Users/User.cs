﻿using Core.Domains.Groups;
using Core.Domains.Roles;
using Core.Domains.Users.Events;
using Core.Domains.Users.Exception;

namespace Core.Domains.Users;

public class User : BaseEntity
{
    protected User()
    {
    }

    internal User(string username, Group group)
    {
        AddUsername(username);

        Join(group);

        ChangeStatus(UserStatus.Active);

        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public Guid Id { get; set; } = Guid.NewGuid();

    public HashSet<string> Usernames { get; internal set; } = new();

    public HashSet<Group> Groups { get; internal set; } = new();

    public HashSet<Role> Roles { get; internal set; } = new();

    public UserStatus Status { get; internal set; } = default!;

    public string? StatusChangeReason { get; internal set; }

    public DateTime StatusChangeDateUtc { get; internal set; }

    private void ChangeReason(string? reason) => StatusChangeReason = reason;

    public void AddUsername(string username)
    {
        if (HasUsername(username))
        {
            throw new UserDuplicateIdentifierException(username);
        }

        Usernames.Add(username);
        AddDomainEvent(new UserUsernameAddedEvent(Id, username));
    }

    public void RemoveUsername(string username)
    {
        if (!HasUsername(username)) return;

        if (Usernames.Count == 1)
        {
            throw new UsernameCouldNotBeEmptyException();
        }

        Usernames.Remove(username);
        AddDomainEvent(new UserUsernameRemovedEvent(Id, username));
    }

    public bool HasUsername(string username)
    {
        return Usernames.Any(u => u == username);
    }

    public void Join(Group group)
    {
        if (Has(group)) return;
        Groups.Add(group);
        AddDomainEvent(new UserJoinedGroupEvent(Id, group.Id));
    }

    public void Leave(Group group)
    {
        if (!Has(group)) return;
        if (Groups.Count == 1)
        {
            throw new UsernameGroupCouldNotBeEmptyException();
        }

        Groups.Remove(group);
        AddDomainEvent(new UserRemovedGroupEvent(Id, group.Id));
    }

    public bool Has(Group group) => Groups.Any(g => g == group);

    public void Assign(Role role)
    {
        if (Has(role)) return;
        Roles.Add(role);
        AddDomainEvent(new UserRoleAddedEvent(Id, role.Id));
    }

    public void Revoke(Role role)
    {
        if (!Has(role)) return;
        Roles.Remove(role);
        AddDomainEvent(new UserRoleRemovedEvent(Id, role.Id));
    }

    public bool Has(Role role) =>Roles.Any(r => r == role);

    public void Activate() => ChangeStatus(UserStatus.Active);

    public void Suspend(string? reason = null) => ChangeStatus(UserStatus.Suspend, reason);

    public void Block(string reason) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, string? reason = null)
    {
        Status = status;
        StatusChangeDateUtc = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}