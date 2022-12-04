using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

public sealed record GroupCreatedEvent(GroupId GroupId) : DomainEvent
{
    public override string Name => "group_created";
}