namespace Auth.Domain.Credentials.Events;

public record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, Guid UserId) : DomainEvent;