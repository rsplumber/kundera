using Core.Domains.Contracts;
using Core.Domains.Credentials;

namespace Core.Domains.Sessions.Events;

[Event("session_created")]
public sealed record SessionCreatedEvent(Token Token) : DomainEvent;