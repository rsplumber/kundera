using Redis.OM.Modeling;

namespace Data.Redis.Scopes;

[Document(IndexName = "scopes", StorageType = StorageType.Json, Prefixes = new[] {"scope"})]
public class ScopeDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    [Indexed] public string Status { get; set; }

    [Indexed] public string[] Services { get; set; }

    [Indexed] public string[] Roles { get; set; }
}