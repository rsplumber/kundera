using System.Net;
using Core.Domains.Auth.Credentials.Events;

namespace Core.Domains.Auth.Credentials;

public class Credential : BaseEntity
{
    protected Credential()
    {
    }

    internal Credential(string username, string password, Guid userId)
    {
        Username = username;
        Password = Password.Create(password);
        UserId = userId;
        CreatedDateUtc = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(Id, userId));
    }

    internal Credential(string username, string password, Guid userId, int expireInMinutes) :
        this(username, password, userId)
    {
        if (expireInMinutes > 0)
        {
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(expireInMinutes);
        }
    }

    internal Credential(string username, string password, Guid userId, bool oneTime, int expireInMinutes = 0) :
        this(username, password, userId, expireInMinutes)
    {
        OneTime = oneTime;
    }


    public Guid Id { get; internal set; } = Guid.NewGuid();

    public string Username { get; internal set; } = default!;

    public Guid UserId { get; internal set; }

    public Password Password { get; internal set; } = default!;

    public string? LastIpAddress { get; internal set; }

    public DateTime? LastLoggedInUtc { get; internal set; }

    public DateTime? ExpiresAtUtc { get; internal set; }

    public int? SessionExpireTimeInMinutes { get; internal set; }

    public bool OneTime { get; internal set; }

    public bool SingleSession { get; init; }

    public DateTime CreatedDateUtc { get; internal set; }

    public void UpdateActivityInfo(IPAddress ipAddress)
    {
        LastIpAddress = ipAddress.ToString();
        LastLoggedInUtc = DateTime.UtcNow;
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