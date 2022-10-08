using Tes.Domain.Contracts;

namespace Domain.Permissions;

public class PermissionId : CustomType<string, PermissionId>, IIdentity
{
}