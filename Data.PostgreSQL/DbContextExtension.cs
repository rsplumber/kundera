using Core.Domains;
using DotNetCore.CAP;

namespace Data.PostgreSQL;

internal static class DbContextExtension
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