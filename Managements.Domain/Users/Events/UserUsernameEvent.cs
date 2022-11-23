using Managements.Domain.Contracts;
using Managements.Domain.Users.Types;

namespace Managements.Domain.Users.Events;

[Event("user_username_added")]
public sealed record UserUsernameAddedEvent(UserId Id, Username Username) : DomainEvent;

[Event("user_username_removed")]
public sealed record UserUsernameRemovedEvent(UserId Id, Username Username) : DomainEvent;