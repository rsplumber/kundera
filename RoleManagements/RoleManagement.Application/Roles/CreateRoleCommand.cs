using RoleManagements.Domain;
using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Roles;

public sealed record CreateRoleCommand(Name Name, IList<PermissionId> Permissions , IDictionary<string,string>? Meta = null) : Command;