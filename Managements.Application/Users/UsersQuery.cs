using Mediator;

namespace Managements.Application.Users;

public sealed record UsersQuery : IQuery<IEnumerable<UsersResponse>>;

public sealed record UsersResponse(Guid Id, IEnumerable<string> Usernames);