using Core.Domains.Users.Types;

namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialCreatedEvent(UniqueIdentifier UniqueIdentifier, UserId User) : DomainEvent
{
    public override string Name => "credential_created";
}