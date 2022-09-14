using Tes.Domain.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users.Events;
using Users.Domain.Users.Exception;
using Users.Domain.Users.Types;

namespace Users.Domain.Users;

public class User : AggregateRoot<UserId>
{
    private readonly string? _username;
    private string? _firstname;
    private string? _lastname;
    private readonly string? _phoneNumber;
    private readonly string? _email;
    private readonly string? _nationalCode;
    private string? _statusChangedReason;
    private UserStatus _status;
    private DateTime _statusChangedDate;
    private readonly ICollection<UserGroupId> _userGroups;
    private readonly ICollection<RoleId> _roles;

    protected User()
    {
    }

    private User(UserId id, UserGroupId userGroupId) : base(id)
    {
        _userGroups = new List<UserGroupId>();
        _roles = new List<RoleId>();
        _status = UserStatus.Active;
        JoinGroup(userGroupId);
        AddDomainEvent(new UserCreatedEvent(id));
    }

    private User(Username username, UserGroupId userGroupId) : this(UserId.Generate(), userGroupId)
    {
        _username = username;
    }

    private User(PhoneNumber phoneNumber, UserGroupId userGroupId) : this(UserId.Generate(), userGroupId)
    {
        _phoneNumber = phoneNumber;
    }

    private User(Email email, UserGroupId userGroupId) : this(UserId.Generate(), userGroupId)
    {
        _email = email;
    }

    private User(NationalCode nationalCode, UserGroupId userGroupId) : this(UserId.Generate(), userGroupId)
    {
        _nationalCode = nationalCode;
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

    public static async Task<User> CreateAsync(PhoneNumber phoneNumber, UserGroupId userGroupId, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(phoneNumber);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(phoneNumber);
        }

        return new User(phoneNumber, userGroupId);
    }

    public static async Task<User> CreateAsync(Email email, UserGroupId userGroupId, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(email);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(email);
        }

        return new User(email, userGroupId);
    }

    public static async Task<User> CreateAsync(NationalCode nationalCode, UserGroupId userGroupId, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(nationalCode);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(nationalCode);
        }

        return new User(nationalCode, userGroupId);
    }

    public string? Username => _username;

    public string? Firstname => _firstname;

    public string? Lastname => _lastname;

    public string? PhoneNumber => _phoneNumber;

    public string? Email => _email;

    public string? NationalCode => _nationalCode;

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

    public void ChangeName(Name firstName, Name lastname)
    {
        _firstname = firstName;
        _lastname = lastname;
    }

    public void Activate() => ChangeStatus(UserStatus.Active);

    public void Block(Text? reason = null) => ChangeStatus(UserStatus.Block, reason);

    private void ChangeStatus(UserStatus status, Text? reason = null)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}