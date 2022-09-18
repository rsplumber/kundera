using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record RemovePermissionMetaCommand(PermissionId PermissionId, IDictionary<string, string> Meta) : Command;