using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record RemoveUserUsernameCommand(UserId User, Username Username) : Command;