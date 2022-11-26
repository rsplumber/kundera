using System.Net;

namespace Application.Auth.Events;

public record OnAuthorizeEvent(Guid UserId, Guid ScopeId, Guid ServiceId, string Action, IPAddress IpAddress)
{
    public DateTime DateUtc { get; } = DateTime.UtcNow;
}