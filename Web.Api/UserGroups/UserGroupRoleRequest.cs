﻿using Application.UserGroups;
using Domain.Roles;
using Domain.UserGroups;
using Kite.Web.Requests;

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