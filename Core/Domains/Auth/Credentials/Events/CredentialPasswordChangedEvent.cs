namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialPasswordChangedEvent(UniqueIdentifier UniqueIdentifier) : DomainEvent
{
    public override string Name => "credential_password_changed";
}