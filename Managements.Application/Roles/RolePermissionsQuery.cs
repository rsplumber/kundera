using Kite.CQRS.Contracts;
using Managements.Domain.Roles;

namespace Managements.Application.Roles;

public sealed record RolePermissionsQuery(RoleId Id) : Query<IEnumerable<RolePermissionsResponse>>;

public sealed record RolePermissionsResponse(Guid Id, string Name);