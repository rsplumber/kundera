using Redis.OM.Modeling;

namespace Managements.Data.Permissions;

[Document(IndexName = "permissions", StorageType = StorageType.Json, Prefixes = new[] {"permission"})]
public class PermissionDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; } = default!;

    [Indexed]
    [Searchable(PropertyName = "name_searchable")]
    public string Name { get; set; } = default!;

    public Dictionary<string, string>? Meta { get; set; }
}