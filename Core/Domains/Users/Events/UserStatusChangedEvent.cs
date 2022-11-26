using Core.Domains.Contracts;
using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

[Event("user_status")]
public sealed record UserStatusChangedEvent(UserId UserId, UserStatus UserStatus) : DomainEvent;