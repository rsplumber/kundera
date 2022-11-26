using System.Net;
using Core.Domains.Contracts;
using Core.Domains.Credentials.Events;

namespace Core.Domains.Credentials;

public class Credential : AggregateRoot
{
    protected Credential()
    {
    }

    internal Credential(UniqueIdentifier uniqueIdentifier, string password, Guid userId, IPAddress? lastIpAddress = null)
    {
        Id = uniqueIdentifier;
        UserId = userId;
        Password = Password.Create(password);
        LastIpAddress = lastIpAddress ?? IPAddress.None;
        LastLoggedIn = DateTime.UtcNow;
        CreatedDate = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(uniqueIdentifier, userId));
    }

    internal Credential(UniqueIdentifier uniqueIdentifier, string password, Guid user, int expireInMinutes = 0, IPAddress? lastIpAddress = null) :
        this(uniqueIdentifier, password, user, lastIpAddress)
    {
        if (expireInMinutes > 0)
        {
            ExpiresAt = DateTime.UtcNow.AddMinutes(expireInMinutes);
        }
    }

    internal Credential(UniqueIdentifier uniqueIdentifier, string password, Guid userId, bool oneTime, int expireInMinutes = 0, IPAddress? lastIpAddress = null) :
        this(uniqueIdentifier, password, userId, expireInMinutes, lastIpAddress)
    {
        OneTime = oneTime;
    }


    public UniqueIdentifier Id { get; internal set; } = default!;

    public Guid UserId { get; internal set; }
    
    public Password Password { get; internal set; } = null!;

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