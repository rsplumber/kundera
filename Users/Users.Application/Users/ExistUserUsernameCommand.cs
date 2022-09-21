using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record ExistUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class ExistUserUsernameCommandHandler : CommandHandler<ExistUserUsernameCommand>
{
    public override Task HandleAsync(ExistUserUsernameCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}