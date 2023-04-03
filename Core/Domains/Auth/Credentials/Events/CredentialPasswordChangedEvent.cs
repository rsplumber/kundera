namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialPasswordChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.credentials.password.changed";
}