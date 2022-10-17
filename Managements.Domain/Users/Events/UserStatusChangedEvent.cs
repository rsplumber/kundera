using Managements.Domain.Users.Types;

namespace Managements.Domain.Users.Events;

public record UserStatusChangedEvent(UserId UserId, UserStatus UserStatus) : DomainEvent;