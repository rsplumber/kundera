using Managements.Domain.Contracts;
using Managements.Domain.Groups.Types;

namespace Managements.Domain.Groups.Events;

[Event("group_created")]
public sealed record GroupCreatedEvent(GroupId GroupId) : DomainEvent;