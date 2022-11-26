using ValueOf;

namespace Core.Domains.Roles.Types;

public sealed class RoleId : ValueOf<Guid, RoleId>
{
    public static RoleId Generate() => From(Guid.NewGuid());
}