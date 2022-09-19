using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record EnableUserGroupCommand(UserGroupId UserGroup) : Command;

public sealed record DisableUserGroupCommand(UserGroupId UserGroup) : Command;