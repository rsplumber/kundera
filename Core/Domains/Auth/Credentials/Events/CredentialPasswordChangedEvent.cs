namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialPasswordChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_credential_password_changed";
}