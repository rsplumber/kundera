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

    internal Session(IHashService hashService, Certificate certificate, Credential credential, Scope scope)
    {
        Id = hashService.HashAsync(StaticHashKey, certificate.Token).Result;
        RefreshToken = hashService.HashAsync(StaticHashKey, certificate.RefreshToken).Result;
        Credential = credential;
        Scope = scope;
        User = credential.User;
        TokenExpirationDateUtc = CalculateTokenExpirationDateUtc();
        RefreshTokenExpirationDateUtc = CalculateRefreshTokenExpirationDateUtc();
        CreatedDateUtc = DateTime.UtcNow;
        AddDomainEvent(new SessionCreatedEvent(Id));
    }

    public string Id { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public Credential Credential { get; set; } = default!;

    public Scope Scope { get; set; } = default!;

    public User User { get; set; } = default!;

    public DateTime TokenExpirationDateUtc { get; set; }

    public DateTime RefreshTokenExpirationDateUtc { get; set; }

    public DateTime CreatedDateUtc { get; set; }

    private DateTime CalculateTokenExpirationDateUtc() => Scope.Restricted
        ? DateTime.UtcNow.AddMinutes(Scope.SessionTokenExpireTimeInMinutes)
        : DateTime.UtcNow.AddMinutes(Credential.SessionTokenExpireTimeInMinutes ?? Scope.SessionTokenExpireTimeInMinutes);

    private DateTime CalculateRefreshTokenExpirationDateUtc() => Scope.Restricted
        ? DateTime.UtcNow.AddMinutes(Scope.SessionRefreshTokenExpireTimeInMinutes)
        : DateTime.UtcNow.AddMinutes(Credential.SessionRefreshTokenExpireTimeInMinutes ?? Scope.SessionRefreshTokenExpireTimeInMinutes);
    
    
}