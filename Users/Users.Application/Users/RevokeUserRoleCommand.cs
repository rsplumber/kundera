using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record RevokeUserRoleCommand(UserId User, params RoleId[] Roles) : Command;

internal sealed class RevokeUserRoleCommandHandler : ICommandHandler<RevokeUserRoleCommand, RevokeUserRoleCommandHandler>
{
    public Task<RevokeUserRoleCommandHandler> HandleAsync(RevokeUserRoleCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}