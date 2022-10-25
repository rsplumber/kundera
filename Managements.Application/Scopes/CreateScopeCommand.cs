using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Scopes;

namespace Managements.Application.Scopes;

public sealed record CreateScopeCommand(Name Name) : Command;

internal sealed class CreateScopeCommandHandler : ICommandHandler<CreateScopeCommand>
{
    private readonly IScopeFactory _scopeFactory;
    private readonly IScopeRepository _scopeRepository;


    public CreateScopeCommandHandler(IScopeFactory scopeFactory, IScopeRepository scopeRepository)
    {
        _scopeFactory = scopeFactory;
        _scopeRepository = scopeRepository;
    }

    public async Task HandleAsync(CreateScopeCommand message, CancellationToken cancellationToken = default)
    {
        var scope = await _scopeFactory.CreateAsync(message.Name);
        await _scopeRepository.AddAsync(scope, cancellationToken);
    }
}