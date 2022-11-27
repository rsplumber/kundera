using Core.Domains.Contracts;
using Core.Domains.Users.Types;

namespace Core.Domains.Credentials.Events;

[Event("credential_created")]
public sealed record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, UserId User) : DomainEvent;