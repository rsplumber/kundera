using ValueOf;

namespace Core.Domains.Users.Types;

public sealed class UserId : ValueOf<Guid, UserId>
{
    public static UserId Generate() => From(Guid.NewGuid());
}