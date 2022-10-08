namespace Auth.Domain.Credentials.Events;

public record CredentialPasswordChangedEvent(UniqueIdentifier UniqueIdentifier) : DomainEvent;