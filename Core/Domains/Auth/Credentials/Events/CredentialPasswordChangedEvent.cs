namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialPasswordChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_credentials.password.changed";
}