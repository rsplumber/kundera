using Core.Domains.Contracts;
using Core.Domains.Groups.Types;

namespace Core.Domains.Groups.Events;

[Event("group_created")]
public sealed record GroupCreatedEvent(GroupId GroupId) : DomainEvent;