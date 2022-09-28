using Redis.OM.Modeling;

namespace Users.Data.Redis.Users;

[Document(StorageType = StorageType.Json, Prefixes = new[] {"user"})]
internal sealed class UserDataModel
{
    [RedisIdField] [Indexed] public string Id { get; set; }

    [Indexed] public string[] Usernames { get; set; }

    [Indexed] public string[] UserGroups { get; set; }

    [Indexed] public string[] Roles { get; set; }

    [Indexed] public string Status { get; set; }

    public string? StatusChangedReason { get; set; }

    public DateTime StatusChangedDate { get; set; }
}