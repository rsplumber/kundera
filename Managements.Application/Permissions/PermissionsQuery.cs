using Mediator;

namespace Managements.Application.Permissions;

public sealed record PermissionsQuery : IQuery<IEnumerable<PermissionsResponse>>
{
    public string? Name { get; init; }
}

public sealed record PermissionsResponse(Guid Id, string Name);