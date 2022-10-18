using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Scopes;

namespace Managements.Application.Scopes;

public sealed record CreateScopeCommand(Name Name) : Command;

internal sealed class CreateScopeCommandHandler : ICommandHandler<CreateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public CreateScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public async Task HandleAsync(CreateScopeCommand message, CancellationToken cancellationToken = default)
    {
        var scope = await Scope.FromAsync(message.Name, _scopeRepository);
        await _scopeRepository.AddAsync(scope, cancellationToken);
    }
}