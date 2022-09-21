using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record AddUserUsernameCommand(UserId User, Username Username) : Command;

internal sealed class AddUserUsernameCommandHandler : ICommandHandler<AddUserUsernameCommand, AddUserUsernameCommandHandler>
{
    public Task<AddUserUsernameCommandHandler> HandleAsync(AddUserUsernameCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}