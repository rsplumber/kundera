using Redis.OM.Modeling;

namespace Users.Data.Redis.UserGroups;

[Document(IndexName = "userGroups", StorageType = StorageType.Json, Prefixes = new[] {"userGroups"})]
internal sealed class UserGroupDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    [Searchable] public string Name { get; set; }

    public string Description { get; set; }

    [Indexed] public string? Parent { get; set; }

    [Indexed] public string Status { get; set; }

    public DateTime StatusChangedDate { get; set; }

    [Indexed] public string[] Roles { get; set; }
}