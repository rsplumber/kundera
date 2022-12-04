using System.Net;
using Core.Domains.Auth.Credentials.Events;
using Core.Domains.Users.Types;

namespace Core.Domains.Auth.Credentials;

public class Credential : AggregateRoot
{
    protected Credential()
    {
    }

    internal Credential(UniqueIdentifier uniqueIdentifier, string password, UserId user, IPAddress? lastIpAddress = null)
    {
        Id = uniqueIdentifier;
        User = user;
        Password = Password.Create(password);
        LastIpAddress = lastIpAddress ?? IPAddress.None;
        LastLoggedIn = DateTime.UtcNow;
        CreatedDate = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(uniqueIdentifier, user));
    }

    internal Credential(UniqueIdentifier uniqueIdentifier, string password, UserId user, int expireInMinutes = 0, IPAddress? lastIpAddress = null) :
        this(uniqueIdentifier, password, user, lastIpAddress)
    {
        if (expireInMinutes > 0)
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(expireInMinutes);
        }
    }

    internal Credential(UniqueIdentifier uniqueIdentifier, string password, UserId user, bool oneTime, int expireInMinutes = 0, IPAddress? lastIpAddress = null) :
        this(uniqueIdentifier, password, user, expireInMinutes, lastIpAddress)
    {
        OneTime = oneTime;
    }


    public UniqueIdentifier Id { get; internal set; } = default!;

    public UserId User { get; internal set; } = default!;

    public Password Password { get; internal set; } = default!;

    public IPAddress LastIpAddress { get; internal set; } = default!;

    public DateTime LastLoggedIn { get; internal set; }

    public DateTime? ExpiresAt { get; internal set; }

    public bool OneTime { get; internal set; }

    public DateTime CreatedDate { get; internal set; }

    public void UpdateActivityInfo(IPAddress ipAddress)
    {
        LastIpAddress = ipAddress;
        LastLoggedIn = DateTime.UtcNow;
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