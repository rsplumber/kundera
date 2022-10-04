using Authentication.Domain.Types;

namespace Authentication.Domain.Events;

public record CredentialPasswordChangedEvent(UniqueIdentifier UniqueIdentifier) : DomainEvent;