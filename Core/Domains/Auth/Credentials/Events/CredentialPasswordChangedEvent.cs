namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialPasswordChangedEvent(CredentialId CredentialId) : DomainEvent
{
    public override string Name => "credential_password_changed";
}