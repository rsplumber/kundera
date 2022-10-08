using Tes.Domain.Contracts;

namespace Domain.Users;

public class UserId : CustomType<Guid, UserId>, IIdentity
{
    public static UserId Generate() => From(Guid.NewGuid());
}