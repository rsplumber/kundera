using System.Net;
using Authentication.Domain.Events;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Types;
using Tes.Domain.Contracts;

namespace Authentication.Domain;

public class Credential : AggregateRoot<UniqueIdentifier>
{
    private readonly Guid _userId;
    private readonly Password _password;
    private string _lastIpAddress;
    private DateTime _lastLoggedIn;

    protected Credential()
    {
    }

    private Credential(UniqueIdentifier uniqueIdentifier, Password password, UserId user, IPAddress? lastIpAddress = null) : base(uniqueIdentifier)
    {
        _userId = user;
        _password = password;
        _lastIpAddress = lastIpAddress is not null ? lastIpAddress.ToString() : IPAddress.None.ToString();
        _lastLoggedIn = DateTime.UtcNow;
        AddDomainEvent(new CredentialCreatedEvent(uniqueIdentifier, user));
    }

    public static async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier,
        Password password,
        UserId user,
        IPAddress ipAddress,
        ICredentialRepository credentialRepository)
    {
        var exists = await credentialRepository.ExistsAsync(uniqueIdentifier);
        if (exists)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        return new(uniqueIdentifier, password, user, ipAddress);
    }

    public static async Task<Credential> CreateAsync(UniqueIdentifier uniqueIdentifier,
        Password password,
        UserId user,
        ICredentialRepository credentialRepository)
    {
        var exists = await credentialRepository.ExistsAsync(uniqueIdentifier);
        if (exists)
        {
            throw new DuplicateUniqueIdentifierException(uniqueIdentifier);
        }

        return new(uniqueIdentifier, password, user);
    }

    public UserId User => _userId;

    public Password Password => _password;

    public IPAddress LastIpAddress => IPAddress.Parse(_lastIpAddress);

    public DateTime LastLoggedIn => _lastLoggedIn;

    public void UpdateActivityInfo(IPAddress ipAddress)
    {
        _lastLoggedIn = DateTime.UtcNow;
        _lastIpAddress = ipAddress.ToString();
    }
}