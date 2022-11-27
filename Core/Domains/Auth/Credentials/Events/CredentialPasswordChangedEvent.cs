using Core.Domains.Contracts;

namespace Core.Domains.Credentials.Events;

[Event("credential_password_changed")]
public sealed record CredentialPasswordChangedEvent(UniqueIdentifier UniqueIdentifier) : DomainEvent;