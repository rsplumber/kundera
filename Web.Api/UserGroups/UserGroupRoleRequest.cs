using Kite.Web.Requests;
using Managements.Application.UserGroups;
using Managements.Domain.Roles;
using Managements.Domain.UserGroups;

namespace Web.Api.UserGroups;

public record AssignUserGroupRoleRequest(List<string> RoleIds) : IWebRequest
{
    public AssignUserGroupRoleCommand ToCommand(Guid userId) => new(UserGroupId.From(userId),
        RoleIds.Select(RoleId.From)
            .ToArray());
}

public record RevokeUserGroupRoleRequest(List<string> RoleIds) : IWebRequest
{
    public RevokeUserGroupRoleCommand ToCommand(Guid userId) => new(UserGroupId.From(userId),
        RoleIds.Select(RoleId.From)
            .ToArray());
}