using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record AddUserUsernameCommand(UserId User, Username Username) : Command;