using Core.Auth.Credentials.Events;
using Core.Users;

namespace Core.Auth.Credentials;

public class Credential : BaseEntity
{
    public Credential()
    {
    }

    public Credential(string username, string password, User user)
    {
        Username = username;
        Password = Password.Create(password);
        User = user;
        CreatedDateUtc = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(Id, user.Id));
    }

    public Credential(string username, string password, User user, int expireInMinutes) :
        this(username, password, user)
    {
        if (expireInMinutes > 0)
        {
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(expireInMinutes);
        }
    }

    public Credential(string username, string password, User user, bool oneTime, int expireInMinutes = 0) :
        this(username, password, user, expireInMinutes)
    {
        OneTime = oneTime;
    }


    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = default!;

    public User User { get; set; } = default!;

    public Password Password { get; set; } = default!;

    public DateTime? ExpiresAtUtc { get; set; }

    public int? SessionTokenExpireTimeInMinutes { get; set; }

    public int? SessionRefreshTokenExpireTimeInMinutes { get; set; }

    public bool OneTime { get; set; }

    public bool SingleSession { get; init; }

    public DateTime CreatedDateUtc { get; set; }

    public void ChangePassword(string password, string newPassword)
    { 
        Password = Password.Create(newPassword);
        AddDomainEvent(new CredentialPasswordChangedEvent(Id));
    }
    public void ChangeUsername(string username)
    { 
        Username = username;
        AddDomainEvent(new CredentialUsernameChangedEvent(Id));
    }

    public bool Expired() => ExpiresAtUtc is not null && DateTime.UtcNow >= ExpiresAtUtc;
}