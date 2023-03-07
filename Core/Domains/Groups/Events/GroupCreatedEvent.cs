﻿namespace Core.Domains.Groups.Events;

public sealed record GroupCreatedEvent(Guid Id) : DomainEvent
{
    public override string Name => "kundera_groups.created";
}