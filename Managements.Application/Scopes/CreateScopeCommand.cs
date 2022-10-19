using Kite.CQRS;
using Kite.CQRS.Contracts;
using Kite.Hashing;
using Managements.Domain;
using Managements.Domain.Scopes;

namespace Managements.Application.Scopes;

public sealed record CreateScopeCommand(Name Name) : Command;

internal sealed class CreateScopeCommandHandler : ICommandHandler<CreateScopeCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IHashService _hashService;

    public CreateScopeCommandHandler(IScopeRepository scopeRepository, IHashService hashService)
    {
        _scopeRepository = scopeRepository;
        _hashService = hashService;
    }

    public async Task HandleAsync(CreateScopeCommand message, CancellationToken cancellationToken = default)
    {
        var scope = await Scope.FromAsync(message.Name, _hashService, _scopeRepository);
        await _scopeRepository.AddAsync(scope, cancellationToken);
    }
}