using System.Net;
using Core.Domains.Credentials;
using Core.Domains.Scopes.Types;

namespace Application.Auth.Authentications;

public record OnUnAuthenticateEvent
{
    public ScopeId? ScopeId { get; set; }

    public UniqueIdentifier? UniqueIdentifier { get; set; }

    public IPAddress? IpAddress { get; set; }

    public DateTime DateUtc { get; } = DateTime.UtcNow;
}