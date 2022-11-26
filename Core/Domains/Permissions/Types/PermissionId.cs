using ValueOf;

namespace Core.Domains.Permissions.Types;

public sealed class PermissionId : ValueOf<Guid, PermissionId>
{
    public static PermissionId Generate() => From(Guid.NewGuid());
}