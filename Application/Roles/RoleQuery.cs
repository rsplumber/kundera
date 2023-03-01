using Mediator;

namespace Application.Roles;

public sealed record RoleQuery : IQuery<RoleResponse>
{
    public Guid RoleId { get; init; } = default!;
}

public sealed record RoleResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<Guid>? Permissions { get; set; }

    public Dictionary<string, string>? Meta { get; set; }
}