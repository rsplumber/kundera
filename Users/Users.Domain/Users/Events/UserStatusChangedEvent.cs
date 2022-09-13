using Users.Domain.Users.Types;

namespace Users.Domain.Users.Events;

public record UserStatusChangedEvent(UserId UserId, UserStatus UserStatus) : DomainEvent;