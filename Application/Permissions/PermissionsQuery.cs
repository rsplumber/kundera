using Mediator;

namespace Application.Permissions;

public sealed record PermissionsQuery : IQuery<List<PermissionsResponse>>
{
    public string? Name { get; init; }
}

public sealed record PermissionsResponse(Guid Id, string Name);