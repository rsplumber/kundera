using Tes.Web.Validators;
using Users.Application.UserGroups;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Web.Api.UserGroups;

public record AssignUserGroupRoleRequest(List<string> RoleIds) : IWebRequest
{
    public AssignUserGroupRoleCommand ToCommand(Guid userId) => new(UserGroupId.From(userId), RoleIds.Select(RoleId.From).ToArray());
}

public record RevokeUserGroupRoleRequest(List<string> RoleIds) : IWebRequest
{
    public RevokeUserGroupRoleCommand ToCommand(Guid userId) => new(UserGroupId.From(userId), RoleIds.Select(RoleId.From).ToArray());
}