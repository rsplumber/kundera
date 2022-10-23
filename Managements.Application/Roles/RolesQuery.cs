﻿using Kite.CQRS.Contracts;

namespace Managements.Application.Roles;

public sealed record RolesQuery : Query<IEnumerable<RolesResponse>>
{
    public string? Name { get; set; }
}

public sealed record RolesResponse(Guid Id, string Name);