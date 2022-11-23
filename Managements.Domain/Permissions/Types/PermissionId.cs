using ValueOf;

namespace Managements.Domain.Permissions.Types;

public sealed class PermissionId : ValueOf<Guid, PermissionId>
{
    public static PermissionId Generate() => From(Guid.NewGuid());
}