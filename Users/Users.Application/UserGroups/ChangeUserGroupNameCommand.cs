using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record ChangeUserGroupNameCommand(UserGroupId UserGroup, Name Name) : Command;

internal sealed class ChangeUserGroupNameCommandHandler  : ICommandHandler<ChangeUserGroupNameCommand, ChangeUserGroupNameCommandHandler>
{
    public Task<ChangeUserGroupNameCommandHandler> HandleAsync(ChangeUserGroupNameCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}