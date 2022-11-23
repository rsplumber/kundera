using Managements.Domain.Contracts;
using Managements.Domain.Users.Types;

namespace Managements.Domain.Users.Events;

[Event("user_status")]
public sealed record UserStatusChangedEvent(UserId UserId, UserStatus UserStatus) : DomainEvent;