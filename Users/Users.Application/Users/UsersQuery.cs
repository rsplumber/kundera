using Tes.CQRS.Contracts;

namespace Users.Application.Users;

public sealed record UsersQuery : Query<IEnumerable<UsersResponse>>;

public sealed record UsersResponse(Guid Id, IEnumerable<string> Usernames);