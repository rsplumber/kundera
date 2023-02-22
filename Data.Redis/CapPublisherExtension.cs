using Core.Domains;
using DotNetCore.CAP;
using Redis.OM.Searching;

namespace Managements.Data;

internal static class CapPublisherExtension
{
    public static async Task DispatchDomainEventsAsync(this ICapPublisher eventBus, BaseEntity entity)
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