using Mediator;

namespace Queries.Roles;

public sealed record RolesQuery : IQuery<List<RolesResponse>>
{
    public string? Name { get; init; }
}

public sealed record RolesResponse(Guid Id, string Name);