namespace Authentication.Infrastructure.Authorization;

public sealed record SessionCache(List<string> Permissions);