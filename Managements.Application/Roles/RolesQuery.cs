﻿using Mediator;

namespace Managements.Application.Roles;

public sealed record RolesQuery : IQuery<IEnumerable<RolesResponse>>
{
    public string? Name { get; init; }
}

public sealed record RolesResponse(Guid Id, string Name);