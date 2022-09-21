using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record EnableUserGroupCommand(UserGroupId UserGroup) : Command;

public sealed record DisableUserGroupCommand(UserGroupId UserGroup) : Command;

internal sealed class DisableUserGroupCommandHandler : ICommandHandler<DisableUserGroupCommand, DisableUserGroupCommandHandler>
{
    public Task<DisableUserGroupCommandHandler> HandleAsync(DisableUserGroupCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}

internal sealed class EnableUserGroupCommandHandler : ICommandHandler<EnableUserGroupCommand, EnableUserGroupCommandHandler>
{
    public Task<EnableUserGroupCommandHandler> HandleAsync(EnableUserGroupCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}