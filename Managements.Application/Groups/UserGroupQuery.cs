using Kite.CQRS.Contracts;
using Managements.Domain.Groups;

namespace Managements.Application.Groups;

public sealed record GroupQuery(GroupId Id) : Query<GroupResponse>;

public sealed record GroupResponse(Guid Id, string Name, string Status)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }

    public DateTime? StatusChangedDate { get; set; }

    public IEnumerable<Guid> Roles { get; set; }
}