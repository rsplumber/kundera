using Mediator;

namespace Data.Abstractions.Users;

public sealed record UsersQuery : PageableQuery, IQuery<PageableResponse<UsersResponse>>;

public sealed record UsersResponse(Guid Id);