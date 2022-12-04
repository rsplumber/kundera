namespace Core.Domains.Auth.Sessions.Events;

public sealed record SessionCreatedEvent(Token Token) : DomainEvent
{
    public override string Name => "session_created";
}