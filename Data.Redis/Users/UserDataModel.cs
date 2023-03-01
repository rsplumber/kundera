using Redis.OM.Modeling;

namespace Data.Users;

[Document(IndexName = "users", StorageType = StorageType.Json, Prefixes = new[] { "user" })]
internal sealed class UserDataModel
{
    [RedisIdField] [Indexed] public Guid Id { get; set; } = default!;

    [Indexed]
    [Searchable(PropertyName = "usernames_searchable")]
    public List<string> Usernames { get; set; } = default!;

    [Indexed] public List<Guid> Groups { get; set; } = default!;

    [Indexed] public List<Guid>? Roles { get; set; }

    [Indexed] public string Status { get; set; } = default!;

    public string? StatusChangeReason { get; set; }

    public DateTime StatusChangeDateUtc { get; set; } = default!;
}