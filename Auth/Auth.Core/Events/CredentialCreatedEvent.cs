using Auth.Core.Domains;

namespace Auth.Core.Events;

public sealed record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, Guid UserId) : DomainEvent;