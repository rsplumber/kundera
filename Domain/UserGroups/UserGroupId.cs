using Tes.Domain.Contracts;

namespace Domain.UserGroups;

public class UserGroupId : CustomType<Guid, UserGroupId>, IIdentity
{
    public static UserGroupId Generate() => From(Guid.NewGuid());
}