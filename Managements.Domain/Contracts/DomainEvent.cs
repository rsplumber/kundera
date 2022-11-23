namespace Managements.Domain.Contracts;

public interface IDomainEvent
{
    public Guid EventId { get; }

    public DateTime CreatedDateUtc { get; }
}

public record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();

    public DateTime CreatedDateUtc { get; } = DateTime.UtcNow;
}

[AttributeUsage(AttributeTargets.Class)]
public class EventAttribute : Attribute
{
    public EventAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}