using Tes.Domain.Contracts;
using Tes.Events.Contracts;

namespace Auth.Domain.Credentials;

public record DomainEvent : IDomainEvent
{
    public EventId EventId { get; } = EventId.Generate();

    public DateTime CreatedOnUtc { get; } = DateTime.UtcNow;
}