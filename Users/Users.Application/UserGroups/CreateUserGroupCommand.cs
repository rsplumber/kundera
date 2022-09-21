using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record CreateUserGroupCommand(Name Name) : Command;

internal sealed class CreateUserGroupCommandHandler : ICommandHandler<CreateUserGroupCommand, CreateUserGroupCommandHandler>
{
    public Task<CreateUserGroupCommandHandler> HandleAsync(CreateUserGroupCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}