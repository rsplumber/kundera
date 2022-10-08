﻿using Domain.Roles;
using Domain.UserGroups;
using Domain.Users.Events;
using Domain.Users.Exception;
using Domain.Users.Types;
using Tes.Domain.Contracts;

namespace Domain.Users;

public class User : AggregateRoot<UserId>
{
    private readonly List<Username> _usernames = new();
    private readonly List<UserGroupId> _userGroups = new();
    private readonly List<RoleId> _roles = new();
    private UserStatus _status;
    private string? _statusChangedReason;
    private DateTime _statusChangedDate;

    protected User()
    {
    }

    private User(Username username, UserGroupId userGroupId) : base(UserId.Generate())
    {
        AddUsername(username);

        JoinGroup(userGroupId);

        ChangeStatus(UserStatus.Active);

        AddDomainEvent(new UserCreatedEvent(Id));
    }


    public static async Task<User> CreateAsync(Username username, UserGroupId userGroupId, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(username);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(username);
        }

        return new User(username, userGroupId);
    }


    public IReadOnlyCollection<Username> Usernames => _usernames.AsReadOnly();

    public string? Reason => _statusChangedReason;

    public UserStatus Status => _status;

    public DateTime StatusChangedDate => _statusChangedDate;

    public IReadOnlyCollection<UserGroupId> UserGroups => _userGroups.AsReadOnly();

    public IReadOnlyCollection<RoleId> Roles => _roles.AsReadOnly();

    private void ChangeReason(Text? reason) => _statusChangedReason = reason ?? null;

    public void AddUsername(Username username)
    {
        if (Has(username))
        {
            throw new UserDuplicateIdentifierException(username);
        }

        _usernames.Add(username);
    }

    public void RemoveUsername(Username username)
    {
        _usernames.Remove(username);
    }

    public bool Has(Username username)
    {
        return _usernames.Exists(u => u == username);
    }

    public void JoinGroup(UserGroupId userGroup)
    {
        _userGroups.Add(userGroup);
    }

    public void RemoveFromGroup(UserGroupId userGroup)
    {
        _userGroups.Remove(userGroup);
    }

    public void AssignRole(RoleId role)
    {
        _roles.Add(role);
    }

    public void RevokeRole(RoleId role)
    {
        _roles.Remove(role);
    }

    public void Activate(Text? reason = null) => ChangeStatus(UserStatus.Active);

    public void Suspend(Text? reason = null) => ChangeStatus(UserStatus.Suspend);

    public void Block(Text reason) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, Text? reason = null)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}