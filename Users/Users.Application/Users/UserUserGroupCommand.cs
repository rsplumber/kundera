using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.Users;

public sealed record JoinUserToGroupCommand(UserGroupId UserGroupId) : Command;

public sealed record RemoveUserFromGroupCommand(UserGroupId UserGroupId) : Command;