using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record AddPermissionMetaCommand(PermissionId Permission, IDictionary<string, string> Meta) : Command;