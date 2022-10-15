namespace Authentication.Infrastructure.Authorization;

public sealed record SessionCache(Guid UserId, List<string> Permissions);