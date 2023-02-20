using Core.Domains;
using DotNetCore.CAP;

namespace Managements.Data;

internal static class EventsExtension
{
    public static async Task DispatchDomainEventsAsync(this ICapPublisher eventBus, Entity entity)
    {
        if (entity.DomainEvents is null) return;

        var domainEvents = entity.DomainEvents.ToList();
        foreach (var domainEvent in domainEvents)
        {
            var eventAttribute = (DomainEvent)domainEvent;
            await eventBus.PublishAsync(eventAttribute.Name, domainEvent);
        }

        entity.ClearDomainEvent();
    }
}