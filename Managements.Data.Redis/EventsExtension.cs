using System.Reflection;
using DotNetCore.CAP;
using Managements.Domain.Contracts;

namespace Managements.Data;

internal static class EventsExtension
{
    public static async Task DispatchDomainEventsAsync(this ICapPublisher eventBus, Entity entity)
    {
        if (entity.DomainEvents is null) return;

        var domainEvents = entity.DomainEvents.ToList();
        entity.ClearDomainEvent();
        foreach (var domainEvent in domainEvents)
        {
            var attribute = domainEvent.GetType().GetCustomAttribute(typeof(EventAttribute));
            if (attribute is null) continue;
            var eventAttribute = (EventAttribute) attribute;
            await eventBus.PublishAsync(eventAttribute.Name, domainEvent);
        }
    }
}