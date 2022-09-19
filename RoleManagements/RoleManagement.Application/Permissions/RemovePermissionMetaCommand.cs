using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Permissions;

public sealed record RemovePermissionMetaCommand(PermissionId Permission, params string[] MetaKeys) : Command;