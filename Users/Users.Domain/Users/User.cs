using Tes.Domain.Contracts;
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

    protected User()
    {
    }

    private User(UserId id) : base(id)
    {
        AddDomainEvent(new UserCreatedEvent(id));
    }

    private User(Username username) : this(UserId.Generate())
    {
        _username = username;
    }

    private User(PhoneNumber phoneNumber) : this(UserId.Generate())
    {
        _phoneNumber = phoneNumber;
    }

    private User(Email email) : this(UserId.Generate())
    {
        _email = email;
    }

    private User(NationalCode nationalCode) : this(UserId.Generate())
    {
        _nationalCode = nationalCode;
    }

    public async Task<User> CreateAsync(Username username, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(username);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(username);
        }

        return new User(username);
    }

    public async Task<User> CreateAsync(PhoneNumber phoneNumber, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(phoneNumber);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(phoneNumber);
        }

        return new User(phoneNumber);
    }

    public async Task<User> CreateAsync(Email email, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(email);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(email);
        }

        return new User(email);
    }

    public async Task<User> CreateAsync(NationalCode nationalCode, IUserRepository userRepository)
    {
        var exists = await userRepository.ExistsAsync(nationalCode);
        if (exists)
        {
            throw new UserDuplicateIdentifierException(nationalCode);
        }

        return new User(nationalCode);
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

    private void ChangeReason(Text? reason) => _statusChangedReason = reason ?? null;

    private void ChangeStatus(UserStatus status, Text? reason = null)
    {
        _status = status;
        _statusChangedDate = DateTime.UtcNow;
        ChangeReason(reason);
        AddDomainEvent(new UserStatusChangedEvent(Id, status));
    }
}