using System.Net;
using Kite.Events.Contracts;

namespace Auth.Services.Events;

public record OnAuthenticateEvent(Guid ScopeId, string UniqueIdentifier, IPAddress IpAddress) : Event
{
    public DateTime DateUtc { get; } = DateTime.UtcNow;
}