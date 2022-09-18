using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record AddRolePermissionCommand(RoleId RoleId, params PermissionId[] Permissions) : Command;

public sealed record RemoveRolePermissionCommand(RoleId RoleId, params PermissionId[] Permissions) : Command;