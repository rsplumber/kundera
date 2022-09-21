using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record MoveUserGroupParentCommand(UserGroupId UserGroup, UserGroupId From, UserGroupId To) : Command;

internal sealed class MoveUserGroupParentCommandHandler : ICommandHandler<MoveUserGroupParentCommand, MoveUserGroupParentCommandHandler>
{
    public Task<MoveUserGroupParentCommandHandler> HandleAsync(MoveUserGroupParentCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}