using Core.Domains.Roles;
using Core.Domains.Roles.Exceptions;
using Core.Domains.Roles.Types;
using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Core.Domains.Scopes.Types;
using Mediator;

namespace Application.Scopes;

public sealed record AddScopeRoleCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class AddScopeRoleCommandHandler : ICommandHandler<AddScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IRoleRepository _roleRepository;

    public AddScopeRoleCommandHandler(IScopeRepository scopeRepository, IRoleRepository roleRepository)
    {
        _scopeRepository = scopeRepository;
        _roleRepository = roleRepository;
    }

    public async ValueTask<Unit> Handle(AddScopeRoleCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.ScopeId), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var roleId in command.Roles)
        {
            var role = await _roleRepository.FindAsync(RoleId.From(roleId), cancellationToken);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            scope.AddRole(role.Id);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}