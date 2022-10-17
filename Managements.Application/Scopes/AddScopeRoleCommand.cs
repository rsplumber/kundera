using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;

namespace Managements.Application.Scopes;

public sealed record AddScopeRoleCommand(ScopeId Scope, params RoleId[] Roles) : Command;

internal sealed class AddScopeRoleCommandHandler : ICommandHandler<AddScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IRoleRepository _roleRepository;

    public AddScopeRoleCommandHandler(IScopeRepository scopeRepository, IRoleRepository roleRepository)
    {
        _scopeRepository = scopeRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask HandleAsync(AddScopeRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, roleIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var roleId in roleIds)
        {
            var role = await _roleRepository.FindAsync(roleId, cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            scope.AddRole(role.Id);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}