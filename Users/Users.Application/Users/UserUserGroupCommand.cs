using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record JoinUserToGroupCommand(UserId User, UserGroupId UserGroup) : Command;

public sealed record RemoveUserFromGroupCommand(UserId User, UserGroupId UserGroup) : Command;