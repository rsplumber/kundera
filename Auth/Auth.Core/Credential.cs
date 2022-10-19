using System.Net;
using Auth.Core.Events;
using Auth.Core.Exceptions;
using Kite.Domain.Contracts;

namespace Auth.Core;

public class Credential : AggregateRoot<UniqueIdentifier>
{
    private Guid _userId;
    private Password _password;
    private IPAddress? _lastIpAddress;
    private DateTime _lastLoggedIn;
    private DateTime? _expiresAt;
    private bool _oneTime;
    private DateTime _createdDate;

    protected Credential()
    {
    }

    private Credential(UniqueIdentifier uniqueIdentifier, string password, Guid userId, IPAddress? lastIpAddress = null) : base(uniqueIdentifier)
    {
        _userId = userId;
        _password = Password.Create(password);
        _lastIpAddress = lastIpAddress;
        _lastLoggedIn = DateTime.UtcNow;
        _createdDate = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(uniqueIdentifier, userId));
    }

    private Credential(UniqueIdentifier uniqueIdentifier, string password, Guid user, int expirationTimeInSeconds = 0, IPAddress? lastIpAddress = null) :
        this(uniqueIdentifier, password, user, lastIpAddress)
    {
        if (expirationTimeInSeconds > 0)
        {
            _expiresAt = DateTime.UtcNow.AddSeconds(expirationTimeInSeconds);
        }
    }

    private Credential(UniqueIdentifier uniqueIdentifier, string password, Guid userId, bool oneTime, int expirationTimeInSeconds = 0, IPAddress? lastIpAddress = null) :
        this(uniqueIdentifier, password, userId, expirationTimeInSeconds, lastIpAddress)
    {
        _oneTime = oneTime;
    }

    public static async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        IPAddress? ipAddress,
        ICredentialRepository credentialRepository)
    {
        var exists = await credentialRepository.ExistsAsync(uniqueIdentifier);
        if (exists)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        return new(uniqueIdentifier, password, userId, ipAddress);
    }

    public static async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        int expirationTimeInSeconds,
        IPAddress? ipAddress,
        ICredentialRepository credentialRepository)
    {
        var exists = await credentialRepository.ExistsAsync(uniqueIdentifier);
        if (exists)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        return new(uniqueIdentifier, password, userId, expirationTimeInSeconds, ipAddress);
    }

    public static async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier,
        string password,
        Guid userId,
        bool oneTime,
        int expirationTimeInSeconds,
        IPAddress? ipAddress,
        ICredentialRepository credentialRepository)
    {
        var exists = await credentialRepository.ExistsAsync(uniqueIdentifier);
        if (exists)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        return new(uniqueIdentifier, password, userId, oneTime, expirationTimeInSeconds, ipAddress);
    }

    public Guid UserId => _userId;

    public Password Password => _password;

    public IPAddress? LastIpAddress => _lastIpAddress;

    public DateTime LastLoggedIn => _lastLoggedIn;

    public DateTime? ExpiresAt => _expiresAt;

    public bool OneTime => _oneTime;

    public DateTime CreatedDate => _createdDate;

    public void UpdateActivityInfo(IPAddress? ipAddress)
    {
        _lastIpAddress = ipAddress;
        _lastLoggedIn = DateTime.UtcNow;
    }

    public void ChangePassword(string password, string newPassword)
    {
        var oldPassword = Password.From(password, Password.Salt);
        if (Password.Equals(oldPassword))
        {
            _password = Password.Create(newPassword);
        }

        AddDomainEvent(new CredentialPasswordChangedEvent(Id));
    }
}