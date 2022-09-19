using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record SetUserGroupParentCommand(UserGroupId UserGroup, UserGroupId Parent) : Command;