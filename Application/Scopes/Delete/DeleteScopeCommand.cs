using Core.Scopes;
using Core.Scopes.Exceptions;
using Mediator;

namespace Application.Scopes.Delete;

public sealed record DeleteScopeCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;
}

internal sealed class DeleteScopeCommandHandler : ICommandHandler<DeleteScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public DeleteScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(DeleteScopeCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(command.ScopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        await _scopeRepository.DeleteAsync(scope.Id, cancellationToken);

        return Unit.Value;
    }
}