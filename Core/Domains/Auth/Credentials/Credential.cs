using System.Net;
using Core.Domains.Auth.Credentials.Events;
using Core.Domains.Users;

namespace Core.Domains.Auth.Credentials;

public class Credential : BaseEntity
{
    protected Credential()
    {
    }

    internal Credential(string username, string password, User user)
    {
        Username = username;
        Password = Password.Create(password);
        User = user;
        CreatedDateUtc = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(Id, user.Id));
    }

    internal Credential(string username, string password, User user, int expireInMinutes) :
        this(username, password, user)
    {
        if (expireInMinutes > 0)
        {
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(expireInMinutes);
        }
    }

    internal Credential(string username, string password, User user, bool oneTime, int expireInMinutes = 0) :
        this(username, password, user, expireInMinutes)
    {
        OneTime = oneTime;
    }


    public Guid Id { get; internal set; } = Guid.NewGuid();

    public string Username { get; internal set; } = default!;

    public User User { get; internal set; }

    public Password Password { get; internal set; } = default!;

    public CredentialActivity? FirstActivity { get; internal set; }
    
    public CredentialActivity? LastActivity { get; internal set; }
    
    public DateTime? ExpiresAtUtc { get; internal set; }

    public int? SessionExpireTimeInMinutes { get; internal set; }

    public bool OneTime { get; internal set; }

    public bool SingleSession { get; init; }

    public DateTime CreatedDateUtc { get; internal set; }

    public void UpdateFirstActivityInfo(IPAddress ipAddress,string agent)
    {
        if(!IsFirstTimeLoggedIn()) return;
        FirstActivity = new CredentialActivity(Id,ipAddress,agent);
    }

    private bool IsFirstTimeLoggedIn() => FirstActivity is null;
    
    public void UpdateActivityInfo(IPAddress ipAddress,string agent)
    {
        LastActivity = new CredentialActivity(Id,ipAddress,agent);
    }

    public void ChangePassword(string password, string newPassword)
    {
        var oldPassword = Password.From(password, Password.Salt);
        if (Password.Equals(oldPassword))
        {
            Password = Password.Create(newPassword);
        }

        AddDomainEvent(new CredentialPasswordChangedEvent(Id));
    }
}