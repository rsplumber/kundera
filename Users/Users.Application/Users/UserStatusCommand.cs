using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record ActiveUserCommand(UserId UserId, Text? Reason) : Command;

public sealed record SuspendUserCommand(UserId UserId, Text? Reason) : Command;

public sealed record BlockUserCommand(UserId UserId, Text Reason) : Command;