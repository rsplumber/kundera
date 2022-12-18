﻿using Mediator;

namespace Application.Roles;

public sealed record RolePermissionsQuery : IQuery<List<RolePermissionsResponse>>
{
    public Guid RoleId { get; init; } = default!;
}

public sealed record RolePermissionsResponse(Guid Id, string Name);