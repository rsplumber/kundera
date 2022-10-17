using Kite.CQRS.Contracts;

namespace Managements.Application.Permissions;

public sealed record PermissionsQuery : Query<IEnumerable<PermissionsResponse>>
{
    public string? Name { get; set; }
}

public sealed record PermissionsResponse(string Id);