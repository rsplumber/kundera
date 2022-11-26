using Application.Groups;
using Core.Domains.Groups.Types;
using Core.Domains.Roles.Types;
using Kite.Web.Requests;

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