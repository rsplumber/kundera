using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record MoveUserGroupParentCommand(UserGroupId UserGroup, UserGroupId From, UserGroupId To) : Command;