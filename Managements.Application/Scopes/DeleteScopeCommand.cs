using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;

namespace Managements.Application.Scopes;

public sealed record DeleteScopeCommand(ScopeId Id) : Command;

internal sealed class DeleteScopeCommandHandler : ICommandHandler<DeleteScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public DeleteScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async Task HandleAsync(DeleteScopeCommand message, CancellationToken cancellationToken = default)
    {
        var scope = await _scopeRepository.FindAsync(message.Id, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        await _scopeRepository.DeleteAsync(scope.Id, cancellationToken);
    }
}