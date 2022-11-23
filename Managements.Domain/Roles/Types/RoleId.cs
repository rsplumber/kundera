using ValueOf;

namespace Managements.Domain.Roles.Types;

public sealed class RoleId : ValueOf<Guid, RoleId>
{
    public static RoleId Generate() => From(Guid.NewGuid());
}