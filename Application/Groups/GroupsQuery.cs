using Mediator;

namespace Application.Groups;

public sealed record GroupsQuery : IQuery<List<GroupsResponse>>;

public sealed record GroupsResponse(Guid Id, string Name, string Status)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }
}