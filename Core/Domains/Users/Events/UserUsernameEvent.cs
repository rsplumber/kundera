using Core.Domains.Contracts;
using Core.Domains.Users.Types;

namespace Core.Domains.Users.Events;

[Event("user_username_added")]
public sealed record UserUsernameAddedEvent(UserId Id, Username Username) : DomainEvent;

[Event("user_username_removed")]
public sealed record UserUsernameRemovedEvent(UserId Id, Username Username) : DomainEvent;