using Mediator;

namespace Queries.Groups;

public sealed record GroupsQuery : PageableQuery, IQuery<PageableResponse<GroupsResponse>>;

public sealed record GroupsResponse(Guid Id, string Name, string Status)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }
}