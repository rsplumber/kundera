using Core.Domains.Contracts;

namespace Core.Domains.Credentials.Events;

[Event("credential_created")]
public sealed record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, Guid UserId) : DomainEvent;