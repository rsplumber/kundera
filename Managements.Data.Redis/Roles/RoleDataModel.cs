using Redis.OM.Modeling;

namespace Managements.Data.Redis.Roles;

[Document(IndexName = "roles", StorageType = StorageType.Json, Prefixes = new[] {"role"})]
public class RoleDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    public Dictionary<string, string> Meta { get; set; }

    public string[] Permissions { get; set; }
}