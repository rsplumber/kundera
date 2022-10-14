namespace Auth.Domain.Credentials.Events;

public sealed record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, Guid UserId) : DomainEvent;