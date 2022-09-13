using Tes.Domain.Contracts;

namespace Users.Domain;

public class UserGroupId : CustomType<Guid, UserGroupId>, IIdentity
{
    public static UserGroupId Generate() => From(Guid.NewGuid());
}