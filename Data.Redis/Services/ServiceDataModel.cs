using Redis.OM.Modeling;

namespace Data.Services;

[Document(IndexName = "services", StorageType = StorageType.Json, Prefixes = new[] { "service" })]
internal sealed class ServiceDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed]
    [Searchable(PropertyName = "name_searchable")]
    public string Name { get; set; } = default!;

    [Indexed] public string Secret { get; set; } = default!;

    [Indexed] public string Status { get; set; } = default!;
}