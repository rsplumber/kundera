using Domain.Permissions;
using Domain.Roles;
using Tes.CQRS.Contracts;

namespace Application.Roles;

public sealed record ValidateRoleQuery(RoleId Role, PermissionId RequestedPermission) : Query<bool>;