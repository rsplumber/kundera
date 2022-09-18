using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record CreateUserGroupCommand(Name Name, params RoleId[] Roles) : Command;

public sealed record CreateUserGroupParentContainedCommand(Name Name, RoleId Roles, UserGroupId ParentId) : Command;