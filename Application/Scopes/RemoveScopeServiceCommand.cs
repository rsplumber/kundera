using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Core.Domains.Scopes.Types;
using Core.Domains.Services.Types;
using Mediator;

namespace Application.Scopes;

public sealed record RemoveScopeServiceCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

    public Guid[] Services { get; init; } = default!;
}

internal sealed class RemoveScopeServiceCommandHandler : ICommandHandler<RemoveScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public RemoveScopeServiceCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Unit> Handle(RemoveScopeServiceCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.ScopeId), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var service in command.Services)
        {
            scope.RemoveService(ServiceId.From(service));
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}