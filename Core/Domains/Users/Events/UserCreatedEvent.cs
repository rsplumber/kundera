using Core.Domains.Contracts;
using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

[Event("user_created")]
public sealed record UserCreatedEvent(UserId UserId) : DomainEvent;