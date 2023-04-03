namespace Core.Domains.Auth.Credentials.Events;

public sealed record CredentialCreatedEvent(Guid Id, Guid User) : DomainEvent
{
    public override string Name => "kundera.credentials.created";
}