using Domain.Roles;
using Domain.Scopes;
using Domain.Scopes.Exceptions;
using Domain.Scopes.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record AddScopeRoleCommand(ScopeId Scope, params RoleId[] Roles) : Command;

internal sealed class AddScopeRoleCommandHandler : CommandHandler<AddScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IRoleRepository _roleRepository;

    public AddScopeRoleCommandHandler(IScopeRepository scopeRepository, IRoleRepository roleRepository)
    {
        _scopeRepository = scopeRepository;
        _roleRepository = roleRepository;
    }

    public override async Task HandleAsync(AddScopeRoleCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, roleIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var role in roleIds)
        {
            await scope.AddRoleAsync(role, _roleRepository);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}