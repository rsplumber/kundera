using Tes.Domain.Contracts;

namespace Users.Domain;

public class UserId : CustomType<Guid, UserId>, IIdentity
{
    public static UserId Generate() => From(Guid.NewGuid());
}