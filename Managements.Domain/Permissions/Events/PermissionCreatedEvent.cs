﻿namespace Managements.Domain.Permissions.Events;

public record PermissionCreatedEvent(PermissionId Id) : DomainEvent;