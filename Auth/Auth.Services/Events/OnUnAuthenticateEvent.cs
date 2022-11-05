using System.Net;
using Kite.Events.Contracts;

namespace Auth.Services.Events;

public record OnUnAuthenticateEvent : Event
{
    public Guid? ScopeId { get; set; }

    public string? UniqueIdentifier { get; set; }

    public IPAddress? IpAddress { get; set; }

    public DateTime DateUtc { get; } = DateTime.UtcNow;
}