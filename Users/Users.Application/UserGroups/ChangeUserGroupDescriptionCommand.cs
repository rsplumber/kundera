using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record ChangeUserGroupDescriptionCommand(UserGroupId UserGroup, Text Description) : Command;