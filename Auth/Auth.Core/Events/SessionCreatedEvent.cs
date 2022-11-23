using Auth.Core.Entities;
using Managements.Domain.Contracts;

namespace Auth.Core.Events;

[Event("session_created")]
public sealed record SessionCreatedEvent(Token Token) : DomainEvent;