using Tes.Domain.Contracts;

namespace RoleManagements.Domain.UserRoles;

public class UserId : CustomType<Guid, UserId>, IIdentity
{
    public static UserId Generate() => From(Guid.NewGuid());
}