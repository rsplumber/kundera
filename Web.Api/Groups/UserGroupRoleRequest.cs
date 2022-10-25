using Kite.Web.Requests;
using Managements.Application.Groups;
using Managements.Domain.Groups;
using Managements.Domain.Roles;

namespace Web.Api.Groups;

public record AssignGroupRoleRequest(List<Guid> RoleIds) : IWebRequest
{
    public AssignGroupRoleCommand ToCommand(Guid userId) => new(GroupId.From(userId),
        RoleIds.Select(RoleId.From)
            .ToArray());
}

public record RevokeGroupRoleRequest(List<Guid> RoleIds) : IWebRequest
{
    public RevokeGroupRoleCommand ToCommand(Guid userId) => new(GroupId.From(userId),
        RoleIds.Select(RoleId.From)
            .ToArray());
}