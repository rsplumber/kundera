using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Domain;
using Users.Domain.UserGroups;

namespace Users.Application.UserGroups;

public sealed record AssignUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;

public sealed record RevokeUserGroupRoleCommand(UserGroupId UserGroup, params RoleId[] Roles) : Command;

internal sealed class AssignUserGroupRoleCommandHandler : ICommandHandler<AssignUserGroupRoleCommand, AssignUserGroupRoleCommandHandler>
{
    public Task<AssignUserGroupRoleCommandHandler> HandleAsync(AssignUserGroupRoleCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}

internal sealed class RevokeUserGroupRoleCommandHandler : ICommandHandler<RevokeUserGroupRoleCommand, RevokeUserGroupRoleCommandHandler>
{
    public Task<RevokeUserGroupRoleCommandHandler> HandleAsync(RevokeUserGroupRoleCommand message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}