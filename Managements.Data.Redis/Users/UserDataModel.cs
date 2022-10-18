using Redis.OM.Modeling;

namespace Managements.Data.Redis.Users;

[Document(IndexName = "users", StorageType = StorageType.Json, Prefixes = new[] {"user"})]
internal sealed class UserDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; }

    [Indexed] public List<string> Usernames { get; set; }

    [Indexed] public List<Guid> UserGroups { get; set; }

    [Indexed] public List<string> Roles { get; set; }

    [Indexed] public string Status { get; set; }

    public string? StatusChangedReason { get; set; }

    public DateTime StatusChangedDate { get; set; }
}