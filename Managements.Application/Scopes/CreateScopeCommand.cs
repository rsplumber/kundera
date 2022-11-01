using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Scopes;

namespace Managements.Application.Scopes;

public sealed record CreateScopeCommand(Name Name) : Command;

internal sealed class CreateScopeCommandHandler : ICommandHandler<CreateScopeCommand>
{
    private readonly IScopeFactory _scopeFactory;


    public CreateScopeCommandHandler(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task HandleAsync(CreateScopeCommand message, CancellationToken cancellationToken = default)
    {
        await _scopeFactory.CreateAsync(message.Name);
    }
}