using Mediator;

namespace Queries.Users;

public sealed record UsersQuery : PageableQuery, IQuery<PageableResponse<UsersResponse>>;

public sealed record UsersResponse(Guid Id, List<string> Usernames);