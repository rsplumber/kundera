using Core.Scopes;
using Mediator;

namespace Application.Scopes.Create;

public sealed record CreateScopeCommand : ICommand<Scope>
{
    public string Name { get; init; } = default!;

    public int SessionTokenExpireTimeInMinutes { get; init; } = default!;

    public int SessionRefreshTokenExpireTimeInMinutes { get; init; } = default!;
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
        var scope = await _scopeFactory.CreateAsync(command.Name, command.SessionTokenExpireTimeInMinutes, command.SessionRefreshTokenExpireTimeInMinutes);
        return scope;
    }
}