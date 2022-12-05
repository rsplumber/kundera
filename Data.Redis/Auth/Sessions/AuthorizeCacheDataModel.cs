using Redis.OM.Modeling;

namespace Managements.Data.Auth.Sessions;

[Document(IndexName = "auth_cache", StorageType = StorageType.Json, Prefixes = new[] {"auth_cache"})]
internal sealed class AuthorizeCacheDataModel
{
    [RedisIdField] [Indexed] public string Token { get; set; } = default!;

    [Indexed] public string UserId { get; set; } = default!;

    [Indexed] public string UserStatus { get; set; } = default!;

    [Indexed] public string[] UserRoles { get; set; } = default!;

    [Indexed] public string[] Permissions { get; set; } = default!;

    [Indexed] public string[] ScopeRoles { get; set; } = default!;

    [Indexed] public string[] ScopeServices { get; set; } = default!;

    [Indexed] public DateTime ExpiresAt { get; set; } = default!;
}