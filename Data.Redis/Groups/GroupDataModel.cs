using Redis.OM.Modeling;

namespace Managements.Data.Groups;

[Document(IndexName = "groups", StorageType = StorageType.Json, Prefixes = new[] { "groups" })]
internal sealed class GroupDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; } = default!;

    [Indexed]
    [Searchable(PropertyName = "name_searchable")]
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    [Indexed] public Guid? Parent { get; set; }

    [Indexed] public string Status { get; set; } = default!;

    [Indexed] public List<Guid>? Children { get; set; }

    public DateTime StatusChangeDate { get; set; } = default!;

    [Indexed] public List<Guid> Roles { get; set; } = default!;
}