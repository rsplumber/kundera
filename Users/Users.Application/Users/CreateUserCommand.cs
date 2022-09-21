using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record CreateUserCommand(Username Username, UserGroupId UserGroup) : Command;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, CreateUserCommandHandler>
{
    public Task<CreateUserCommandHandler> HandleAsync(CreateUserCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}