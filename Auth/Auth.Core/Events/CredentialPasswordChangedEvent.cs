using Auth.Core.Domains;

namespace Auth.Core.Events;

public sealed record CredentialPasswordChangedEvent(UniqueIdentifier UniqueIdentifier) : DomainEvent;