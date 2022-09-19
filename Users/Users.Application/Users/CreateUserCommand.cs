using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;
using Users.Domain.Users.Types;

namespace Users.Application.Users;

public sealed record CreateUserCommand(Username Username, UserGroupId UserGroup) : Command;