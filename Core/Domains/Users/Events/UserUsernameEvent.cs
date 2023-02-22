﻿namespace Core.Domains.Users.Events;

public sealed record UserUsernameAddedEvent(Guid Id, string Username) : DomainEvent
{
    public override string Name => "kundera_user_username_added";
}

public sealed record UserUsernameRemovedEvent(Guid Id, string Username) : DomainEvent
{
    public override string Name => "kundera_user_username_removed";
}