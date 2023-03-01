using Core.Domains.Scopes;
using Mediator;

namespace Application.Scopes;

public sealed record CreateScopeCommand : ICommand<Scope>
{
    public string Name { get; init; } = default!;
}

internal sealed class CreateScopeCommandHandler : ICommandHandler<CreateScopeCommand, Scope>
{
    private readonly IScopeFactory _scopeFactory;

    public CreateScopeCommandHandler(IScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async ValueTask<Scope> Handle(CreateScopeCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeFactory.CreateAsync(command.Name);
        return scope;
    }
}