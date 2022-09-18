using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record EnableUserGroupCommand(UserGroupId UserGroupId) : Command;

public sealed record DisableUserGroupCommand(UserGroupId UserGroupId) : Command;