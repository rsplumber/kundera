using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record RemoveUserGroupParentCommand(UserGroupId UserGroup, UserGroupId Parent) : Command;

internal sealed class RemoveUserGroupParentCommandHandler : ICommandHandler<RemoveUserGroupParentCommand, RemoveUserGroupParentCommandHandler>
{
    public Task<RemoveUserGroupParentCommandHandler> HandleAsync(RemoveUserGroupParentCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}