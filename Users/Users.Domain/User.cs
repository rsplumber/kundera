using Tes.Domain.Contracts;
using Users.Domain.Events;
using Users.Domain.Types;

namespace Users.Domain;

public class User : AggregateRoot<UserId>
{
    private readonly string? _username;
    private string? _firstname;
    private string? _lastname;
    private readonly string? _phoneNumber;
    private readonly string? _email;

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

    public string? Username => _username;

    public string? Firstname => _firstname;

    public string? Lastname => _lastname;

    public string? PhoneNumber => _phoneNumber;

    public string? Email => _email;
    
    public void ChangeName(Name firstname,Name lastname)
    {
        _firstname = firstname;
        _lastname = lastname;
        AddDomainEvent(new UserChangedNameEvent(Id));
    }
    
    
}