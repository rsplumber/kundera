using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Exceptions;
using RoleManagements.Domain.Scopes.Types;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

public sealed record RemoveScopeServiceCommand(ScopeId Scope, params ServiceId[] Services) : Command;

internal sealed class RemoveScopeServiceCommandHandler : CommandHandler<RemoveScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeServiceCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public override async Task HandleAsync(RemoveScopeServiceCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, serviceIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var service in serviceIds)
        {
            scope.RemoveService(service);
        }
        
        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}