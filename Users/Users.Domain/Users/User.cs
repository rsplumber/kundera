using Tes.Domain.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users.Events;
using Users.Domain.Users.Exception;
using Users.Domain.Users.Types;

namespace Users.Domain.Users;

public class User : AggregateRoot<UserId>
{
    private readonly string _username;
    private UserStatus _status;
    private string? _statusChangedReason;
    private DateTime _statusChangedDate;
    private readonly ICollection<UserGroupId> _userGroups;
    private readonly ICollection<RoleId> _roles;

    protected User()
    {
    }

    private User(Username username, UserGroupId userGroupId) : base(UserId.Generate())
    {
        _userGroups = new List<UserGroupId>();
        _roles = new List<RoleId>();
        _username = username;
        _status = UserStatus.Active;
        JoinGroup(userGroupId);
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


    public string Username => _username;

    public string? Reason => _statusChangedReason;

    public UserStatus Status => _status;

    public DateTime StatusChangedDate => _statusChangedDate;

    public IReadOnlyCollection<UserGroupId> UserGroups => (IReadOnlyCollection<UserGroupId>) _userGroups;

    public IReadOnlyCollection<RoleId> Roles => (IReadOnlyCollection<RoleId>) _roles;

    private void ChangeReason(Text? reason) => _statusChangedReason = reason ?? null;

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

    public void Activate() => ChangeStatus(UserStatus.Active);

    public void Suspend() => ChangeStatus(UserStatus.Suspend);

    public void Block(Text? reason = null) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, Text? reason = null)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}