using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Roles.Types;

public class RoleId : CustomType<string, RoleId>, IIdentity
{
}