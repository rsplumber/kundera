using Authentication.Domain.Types;

namespace Authentication.Domain.Events;

public record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, UserId UserId) : DomainEvent;