using Tes.Domain.Contracts;
using Users.Domain.Users.Events;
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

    public User() : base(UserId.Generate())
    {
    }

    public User(string username) : base(UserId.Generate())
    {
        _username = username;
        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public User(PhoneNumber phoneNumber) : base(UserId.Generate())
    {
        _phoneNumber = phoneNumber;
        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public User(Email email) : base(UserId.Generate())
    {
        _email = email;
        AddDomainEvent(new UserCreatedEvent(Id));
    }
    
    public User(NationalCode nationalCode) : base(UserId.Generate())
    {
        _nationalCode = nationalCode;
        AddDomainEvent(new UserCreatedEvent(Id));
    }

    public string? Username => _username;

    public string? Firstname => _firstname;

    public string? Lastname => _lastname;

    public string? PhoneNumber => _phoneNumber;

    public string? Email => _email;
}