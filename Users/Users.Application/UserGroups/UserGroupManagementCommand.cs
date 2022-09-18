using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record ChangeUserGroupNameCommand(UserGroupId UserGroupId, Name Name) : Command;

public sealed record ChangeUserGroupDescriptionCommand(UserGroupId UserGroupId, Text Description) : Command;

public sealed record SetParentToUserGroupCommand(UserGroupId UserGroupId, UserGroupId ParentId) : Command;