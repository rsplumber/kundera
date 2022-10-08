﻿namespace Domain.UserGroups.Events;

public record UserGroupParentChangedEvent(UserGroupId GroupId, UserGroupId? Parent, UserGroupId? PreviousParent) : DomainEvent;