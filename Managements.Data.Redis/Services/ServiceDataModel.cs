using Redis.OM.Modeling;

namespace Managements.Data.Redis.Services;

[Document(IndexName = "services", StorageType = StorageType.Json, Prefixes = new[] {"service"})]
public class ServiceDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    [Indexed] public string Status { get; set; }
}