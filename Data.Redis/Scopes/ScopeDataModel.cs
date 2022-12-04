using Redis.OM.Modeling;

namespace Managements.Data.Scopes;

[Document(IndexName = "scopes", StorageType = StorageType.Json, Prefixes = new[] {"scope"})]
public class ScopeDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; } = default!;

    [Indexed] public string Name { get; set; } = default!;

    [Indexed] public string Secret { get; set; } = default!;

    [Indexed] public string Status { get; set; } = default!;

    [Indexed] public List<Guid>? Services { get; set; }

    [Indexed] public List<Guid>? Roles { get; set; }
}