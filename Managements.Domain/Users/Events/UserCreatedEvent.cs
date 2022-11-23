using Managements.Domain.Contracts;
using Managements.Domain.Users.Types;

namespace Managements.Domain.Users.Events;

[Event("user_created")]
public sealed record UserCreatedEvent(UserId UserId) : DomainEvent;