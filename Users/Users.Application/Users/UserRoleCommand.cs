using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record AssignUserRoleCommand(UserId UserId, params RoleId[] Roles) : Command;

public sealed record RevokeUserRoleCommand(UserId UserId, params RoleId[] Roles) : Command;