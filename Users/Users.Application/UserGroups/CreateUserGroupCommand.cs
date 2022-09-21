using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;

namespace Users.Application.UserGroups;

public sealed record CreateUserGroupCommand(Name Name) : Command;

internal sealed class CreateUserGroupCommandHandler : CommandHandler<CreateUserGroupCommand>
{
    public override async Task HandleAsync(CreateUserGroupCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}