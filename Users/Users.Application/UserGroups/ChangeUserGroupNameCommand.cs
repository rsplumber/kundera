using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record ChangeUserGroupNameCommand(UserGroupId UserGroup, Name Name) : Command;