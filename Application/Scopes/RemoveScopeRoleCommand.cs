using Core.Domains.Roles.Types;
using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Core.Domains.Scopes.Types;
using Mediator;

namespace Application.Scopes;

public sealed record RemoveScopeRoleCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

    public Guid[] Roles { get; init; } = default!;
}

internal sealed class RemoveScopeRoleCommandHandler : ICommandHandler<RemoveScopeRoleCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeRoleCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(RemoveScopeRoleCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.ScopeId), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var role in command.Roles)
        {
            scope.RemoveRole(RoleId.From(role));
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}