using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record SetUserGroupParentCommand(UserGroupId UserGroup, UserGroupId Parent) : Command;

internal sealed class SetUserGroupParentCommandHandler : ICommandHandler<SetUserGroupParentCommand>
{
    public Task HandleAsync(SetUserGroupParentCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}