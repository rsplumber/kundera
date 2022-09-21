using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.Users;

namespace Users.Application.Users;

public sealed record AssignUserRoleCommand(UserId User, params RoleId[] Roles) : Command;

internal sealed class AssignUserRoleCommandHandler : ICommandHandler<AssignUserRoleCommand, AssignUserRoleCommandHandler>
{
    public Task<AssignUserRoleCommandHandler> HandleAsync(AssignUserRoleCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}