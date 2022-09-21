using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record ExistUserUsernameCommand(UserId User, Username Username) : Command;