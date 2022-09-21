using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record RemoveUserGroupParentCommand(UserGroupId UserGroup, UserGroupId Parent) : Command;

internal sealed class RemoveUserGroupParentCommandHandler : CommandHandler<RemoveUserGroupParentCommand>
{
    public override Task HandleAsync(RemoveUserGroupParentCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}