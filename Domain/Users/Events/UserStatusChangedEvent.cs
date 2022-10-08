using Domain.Users.Types;

namespace Domain.Users.Events;

public record UserStatusChangedEvent(UserId UserId, UserStatus UserStatus) : DomainEvent;