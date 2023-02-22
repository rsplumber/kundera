﻿namespace Core.Domains.Groups.Events;

public record GroupParentChangedEvent(Guid GroupId, Guid? ParentId, Guid? PreviousParentId) : DomainEvent
{
    public override string Name => "kundera_group_parent_changed";
}