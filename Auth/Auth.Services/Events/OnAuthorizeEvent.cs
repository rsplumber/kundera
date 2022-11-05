using System.Net;
using Kite.Events.Contracts;

namespace Auth.Services.Events;

public record OnAuthorizeEvent(Guid UserId, Guid ScopeId, Guid ServiceId, string Action, IPAddress IpAddress) : Event
{
    public DateTime DateUtc { get; } = DateTime.UtcNow;
}