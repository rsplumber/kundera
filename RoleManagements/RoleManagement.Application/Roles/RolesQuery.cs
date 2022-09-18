using RoleManagements.Domain;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record RolesQuery : Query<RolesResponse>
{
    public Name? Name { get; set; }
};

public sealed record RolesResponse(string Id);