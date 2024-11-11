namespace Core.Auth.Credentials.Events;

public sealed record CredentialUsernameChangedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera.credentials.username.changed";
}