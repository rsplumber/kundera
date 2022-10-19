using Kite.CQRS.Contracts;
using Managements.Domain.Roles;

namespace Managements.Application.Roles;

public sealed record RoleQuery(RoleId RoleId) : Query<RoleResponse>;

public sealed record RoleResponse(Guid Id, string Name)
{
    public IEnumerable<Guid>? Permissions { get; set; }

    public Dictionary<string, string>? Meta { get; set; }
}