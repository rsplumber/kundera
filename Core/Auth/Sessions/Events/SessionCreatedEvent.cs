namespace Core.Auth.Sessions.Events;

public sealed record SessionCreatedEvent(string Token) : DomainEvent
{
    public override string Name => "kundera_sessions.created";
}