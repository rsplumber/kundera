using System.Net;
using Core.Domains.Scopes.Types;

namespace Application.Auth.Authentications;

public record OnAuthenticateEvent(ScopeId Scope, string UniqueIdentifier, IPAddress IpAddress)
{
    public DateTime DateUtc { get; } = DateTime.UtcNow;
}