using Mediator;

namespace Application.Users;

public sealed record UsersQuery : IQuery<List<UsersResponse>>;

public sealed record UsersResponse(Guid Id, IEnumerable<string> Usernames);