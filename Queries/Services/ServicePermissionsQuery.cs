using Mediator;

namespace Queries.Services;

public sealed record ServicePermissionsQuery : IQuery<List<PermissionsResponse>>
{
    public Guid ServiceId { get; set; }

    public string? Name { get; init; }
}

public sealed record PermissionsResponse(Guid Id, string Name);