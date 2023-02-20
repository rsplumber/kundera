using System.Net;
using Core.Domains.Auth.Credentials.Events;
using Core.Domains.Users;
using Core.Domains.Users.Types;

namespace Core.Domains.Auth.Credentials;

public class Credential : AggregateRoot
{
    protected Credential()
    {
    }

    internal Credential(string username, string password, UserId user)
    {
        Username = Username.From(username);
        Password = Password.Create(password);
        User = user;
        CreatedDateUtc = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(Id, user));
    }

    internal Credential(string username, string password, UserId user, int expireInMinutes) :
        this(username, password, user)
    {
        if (expireInMinutes > 0)
        {
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(expireInMinutes);
        }
    }

    internal Credential(string username, string password, UserId user, bool oneTime, int expireInMinutes = 0) :
        this(username, password, user, expireInMinutes)
    {
        OneTime = oneTime;
    }


    public CredentialId Id { get; internal set; } = CredentialId.Generate();

    public Username Username { get; internal set; } = default!;

    public UserId User { get; internal set; } = default!;

    public Password Password { get; internal set; } = default!;

    public IPAddress LastIpAddress { get; internal set; } = IPAddress.None;

    public DateTime? LastLoggedInUtc { get; internal set; }

    public DateTime? ExpiresAtUtc { get; internal set; }

    public int? SessionExpireTimeInMinutes { get; internal set; }

    public bool OneTime { get; internal set; }

    public bool SingleSession { get; init; }

    public DateTime CreatedDateUtc { get; internal set; }

    public void UpdateActivityInfo(IPAddress ipAddress)
    {
        LastIpAddress = ipAddress;
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