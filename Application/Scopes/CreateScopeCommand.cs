using Domain;
using Domain.Scopes;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record CreateScopeCommand(Name Name) : Command;

internal sealed class CreateScopeCommandHandler : CommandHandler<CreateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;

    public CreateScopeCommandHandler(IScopeRepository scopeRepository)
    {
        _scopeRepository = scopeRepository;
    }

    public override async Task HandleAsync(CreateScopeCommand message, CancellationToken cancellationToken = default)
    {
        var scope = await Scope.FromAsync(message.Name, _scopeRepository);
        await _scopeRepository.AddAsync(scope, cancellationToken);
    }
}