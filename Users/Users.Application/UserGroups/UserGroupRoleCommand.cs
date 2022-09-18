using Tes.CQRS.Contracts;
using Users.Domain;

namespace Users.Application.UserGroups;

public sealed record AssignUserGroupRoleCommand(params RoleId[] Roles) : Command;

public sealed record RevokeUserGroupRoleCommand(params RoleId[] Roles) : Command;