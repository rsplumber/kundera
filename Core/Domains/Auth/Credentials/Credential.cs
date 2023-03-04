using System.Net;
using Core.Domains.Auth.Credentials.Events;
using Core.Domains.Users;

namespace Core.Domains.Auth.Credentials;

public class Credential : BaseEntity
{
    public Credential()
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


    public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = default!;

    public User User { get; set; }

    public Password Password { get; set; } = default!;


    public Guid? FirstActivityId { get; set; }

    public AuthActivity? FirstActivity { get; set; }

    public Guid? LastActivityId { get; set; }

    public AuthActivity? LastActivity { get; set; }

    public DateTime? ExpiresAtUtc { get; set; }

    public int? SessionExpireTimeInMinutes { get; set; }

    public bool OneTime { get; set; }

    public bool SingleSession { get; init; }

    public DateTime CreatedDateUtc { get; set; }

    public void UpdateFirstActivityInfo(IPAddress ipAddress, string agent)
    {
        if (!IsFirstTimeLoggedIn()) return;
        FirstActivity = new AuthActivity(ipAddress, agent);
    }

    private bool IsFirstTimeLoggedIn() => FirstActivity is null;

    public void UpdateActivityInfo(IPAddress ipAddress, string agent)
    {
        LastActivity = new AuthActivity(ipAddress, agent);
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