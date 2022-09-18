using RoleManagements.Domain;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record CreatePermissionCommand(Name Name, IDictionary<string, string>? Meta = null) : Command;