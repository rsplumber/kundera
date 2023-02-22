namespace Core.Domains.Auth.Sessions.Events;

public sealed record SessionCreatedEvent(string Token) : DomainEvent
{
    public override string Name => "kundera_session_created";
}