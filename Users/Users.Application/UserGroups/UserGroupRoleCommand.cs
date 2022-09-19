using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record AssignUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;

public sealed record RevokeUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;