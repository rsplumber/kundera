using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record ActiveUserCommand(UserId User, Text? Reason) : Command;

public sealed record SuspendUserCommand(UserId User, Text? Reason) : Command;

public sealed record BlockUserCommand(UserId User, Text Reason) : Command;