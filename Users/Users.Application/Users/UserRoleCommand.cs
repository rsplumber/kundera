using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.Users;

public sealed record AssignUserRoleCommand(params RoleId[] Roles) : Command;

public sealed record RevokeUserRoleCommand(params RoleId[] Roles) : Command;