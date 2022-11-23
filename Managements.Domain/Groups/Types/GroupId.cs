using ValueOf;

namespace Managements.Domain.Groups.Types;

public sealed class GroupId : ValueOf<Guid, GroupId>
{
    public static GroupId Generate() => From(Guid.NewGuid());
}