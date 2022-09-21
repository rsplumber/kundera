using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record ExistUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class ExistUserUsernameCommandHandler : ICommandHandler<ExistUserUsernameCommand, ExistUserUsernameCommandHandler>
{
    public Task<ExistUserUsernameCommandHandler> HandleAsync(ExistUserUsernameCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}