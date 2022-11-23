using Auth.Core.Entities;
using Managements.Domain.Contracts;

namespace Auth.Core.Events;

[Event("credential_created")]
public sealed record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, Guid UserId) : DomainEvent;