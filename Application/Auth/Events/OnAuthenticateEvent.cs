using System.Net;

namespace Application.Auth.Events;

public record OnAuthenticateEvent(Guid ScopeId, string UniqueIdentifier, IPAddress IpAddress)
{
    public DateTime DateUtc { get; } = DateTime.UtcNow;
}