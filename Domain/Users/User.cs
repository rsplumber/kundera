using Domain.Roles;
using Domain.UserGroups;
using Domain.Users.Events;
using Domain.Users.Exception;
using Domain.Users.Types;
using Kite.Domain.Contracts;

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

        AddDomainEvent(new UserUsernameAddedEvent(Id, username));
    }

    public void RemoveUsername(Username username)
    {
        if (!Has(username)) return;
        if (Usernames.Count ==1)
        {
            throw new UsernameCouldNotBeEmptyException();
        }
        _usernames.Remove(username);
        AddDomainEvent(new UserUsernameRemovedEvent(Id, username));
    }

    public bool Has(Username username)
    {
        return _usernames.Any(u => u == username);
    }

    public void JoinGroup(UserGroupId userGroup)
    {
        if (Has(userGroup)) return;
        _userGroups.Add(userGroup);
        AddDomainEvent(new UserUserGroupJoinedEvent(Id, userGroup));
    }

    public void RemoveFromGroup(UserGroupId userGroup)
    {
        if (!Has(userGroup)) return;
        if (UserGroups.Count==1)
        {
            throw new UsernameUserGroupCouldNotBeEmptyException();
        }
        _userGroups.Remove(userGroup);
        AddDomainEvent(new UserUserGroupEventRemovedEvent(Id, userGroup));
    }

    public bool Has(UserGroupId userGroup)
    {
        return _userGroups.Any(id => id == userGroup);
    }

    public void AssignRole(RoleId role)
    {
        if (Has(role)) return;
        _roles.Add(role);
        AddDomainEvent(new UserRoleAddedEvent(Id, role));
    }

    public void RevokeRole(RoleId role)
    {
        if (!Has(role)) return;
        _roles.Remove(role);
        AddDomainEvent(new UserRoleRemovedEvent(Id, role));
    }

    public bool Has(RoleId role)
    {
        return _roles.Any(id => id == role);
    }

    public void Activate() => ChangeStatus(UserStatus.Active, null);

    public void Suspend(Text? reason = null) => ChangeStatus(UserStatus.Suspend, reason);

    public void Block(Text reason) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, Text? reason = null)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}