﻿using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record RolesQuery : Query<IEnumerable<RolesResponse>>
{
    public string? Name { get; set; }
}

public sealed record RolesResponse(string Id);