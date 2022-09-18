using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record JoinUserToGroupCommand(UserId UserId, UserGroupId UserGroupId) : Command;

public sealed record RemoveUserFromGroupCommand(UserId UserId, UserGroupId UserGroupId) : Command;