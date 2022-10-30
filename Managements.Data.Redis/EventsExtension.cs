using Kite.Domain.Contracts;
using Kite.Events;

namespace Managements.Data;

internal static class EventsExtension
{
    public static async Task DispatchDomainEventsAsync<TEntity>(this IEventBus eventBus, Entity<TEntity> entity)
        where TEntity : IEntityIdentity
    {
        if (entity.DomainEvents is null) return;

        var events = entity.DomainEvents.ToList();
        entity.ClearDomainEvent();
        foreach (var domainEvent in events)
        {
            await eventBus.PublishAsync(domainEvent);
        }
    }
}