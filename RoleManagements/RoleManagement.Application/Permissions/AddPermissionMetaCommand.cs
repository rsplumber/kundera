using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record AddPermissionMetaCommand(PermissionId PermissionId, IDictionary<string, string> Meta) : Command;