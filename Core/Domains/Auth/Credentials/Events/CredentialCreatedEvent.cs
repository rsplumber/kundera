using Core.Domains.Users.Types;

namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialCreatedEvent(CredentialId CredentialId, UserId User) : DomainEvent
{
    public override string Name => "credential_created";
}