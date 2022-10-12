namespace Kundera.Management.Users;

public sealed record UsersResponse(Guid Id, IEnumerable<string> Usernames);