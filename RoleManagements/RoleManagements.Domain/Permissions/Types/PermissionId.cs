using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Permissions.Types;

public class PermissionId : CustomType<string, PermissionId>, IIdentity
{
}