using Redis.OM.Modeling;

namespace Managements.Data.Roles;

[Document(IndexName = "roles", StorageType = StorageType.Json, Prefixes = new[] {"role"})]
public class RoleDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed] public string Name { get; set; } = default!;

    public Dictionary<string, string>? Meta { get; set; }

    [Indexed] public List<Guid>? Permissions { get; set; }
}