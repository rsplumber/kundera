using RoleManagements.Domain;
using RoleManagements.Domain.Scopes;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Scopes;

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
        var scope = await Scope.CreateAsync(message.Name, _scopeRepository);
        await _scopeRepository.AddAsync(scope, cancellationToken);
    }
}