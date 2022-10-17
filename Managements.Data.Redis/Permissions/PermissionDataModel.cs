using Redis.OM.Modeling;

namespace Managements.Data.Redis.Permissions;

[Document(IndexName = "permissions", StorageType = StorageType.Json, Prefixes = new[] {"permission"})]
public class PermissionDataModel
{
    [RedisIdField]
    [Indexed]
    public string Id { get; set; }

    public Dictionary<string, string> Meta { get; set; }
}