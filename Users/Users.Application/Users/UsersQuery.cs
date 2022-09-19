using Tes.CQRS.Contracts;

namespace Users.Application.Users;

public sealed record UsersQuery : Query<IEnumerable<UserResponse>>;

public sealed record UsersResponse(string Id, string Username);