using Auth.Core.Entities;
using Managements.Domain.Contracts;

namespace Auth.Core.Events;

[Event("credential_password_changed")]
public sealed record CredentialPasswordChangedEvent(UniqueIdentifier UniqueIdentifier) : DomainEvent;