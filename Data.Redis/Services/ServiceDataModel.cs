using Redis.OM.Modeling;

namespace Managements.Data.Services;

[Document(IndexName = "services", StorageType = StorageType.Json, Prefixes = new[] {"service"})]
public class ServiceDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed] public string Name { get; set; } = default!;

    [Indexed] public string Secret { get; set; } = default!;

    [Indexed] public string Status { get; set; } = default!;
}