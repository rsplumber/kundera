﻿namespace Core.Domains.Groups.Events;

public sealed record GroupStatusChangedEvent(Guid GroupId, GroupStatus Status) : DomainEvent
{
    public override string Name => "kundera_group_status_changed";
}