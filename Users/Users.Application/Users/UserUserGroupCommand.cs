using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain.UserGroups;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record JoinUserToGroupCommand(UserId User, UserGroupId UserGroup) : Command;

public sealed record RemoveUserFromGroupCommand(UserId User, UserGroupId UserGroup) : Command;

internal sealed class JoinUserToGroupCommandHandler : ICommandHandler<JoinUserToGroupCommand, JoinUserToGroupCommandHandler>
{
    public Task<JoinUserToGroupCommandHandler> HandleAsync(JoinUserToGroupCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}

internal sealed class RemoveUserFromGroupCommandHandler : ICommandHandler<RemoveUserFromGroupCommand, RemoveUserFromGroupCommandHandler>
{
    public Task<RemoveUserFromGroupCommandHandler> HandleAsync(RemoveUserFromGroupCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}