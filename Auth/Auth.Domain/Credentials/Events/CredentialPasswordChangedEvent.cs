namespace Auth.Domain.Credentials.Events;

public sealed record CredentialPasswordChangedEvent(UniqueIdentifier UniqueIdentifier) : DomainEvent;