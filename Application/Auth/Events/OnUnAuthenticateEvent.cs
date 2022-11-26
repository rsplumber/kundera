using System.Net;

namespace Application.Auth.Events;

public record OnUnAuthenticateEvent
{
    public Guid? ScopeId { get; set; }

    public string? UniqueIdentifier { get; set; }

    public IPAddress? IpAddress { get; set; }

    public DateTime DateUtc { get; } = DateTime.UtcNow;
}