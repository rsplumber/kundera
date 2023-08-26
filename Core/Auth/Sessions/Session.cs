using Core.Auth.Authorizations;
using Core.Auth.Credentials;
using Core.Auth.Sessions.Events;
using Core.Hashing;
using Core.Scopes;
using Core.Users;

namespace Core.Auth.Sessions;

public class Session : BaseEntity
{
    public const string StaticHashKey = "session_hash_key";

    public Session()
    {
    }

    internal Session(
        IHashService hashService,
        Certificate certificate,
        Credential credential,
        Scope scope,
        User user)
    {
        Id = hashService.HashAsync(StaticHashKey, certificate.Token).Result;
        RefreshToken = hashService.HashAsync(StaticHashKey, certificate.RefreshToken).Result;
        Credential = credential;
        Scope = scope;
        User = user;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public string Id { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public Credential Credential { get; set; } = default!;

    public Scope Scope { get; set; } = default!;

    public User User { get; set; } = default!;

    public DateTime TokenExpirationDateUtc { get; init; } = DateTime.UtcNow;

    public DateTime RefreshTokenExpirationDateUtc { get; init; } = DateTime.UtcNow;

    public DateTime CreatedDateUtc { get; private set; } = DateTime.UtcNow;
}