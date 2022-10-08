using Domain.Scopes;
using Domain.Scopes.Exceptions;
using Domain.Scopes.Types;
using Domain.Services;
using Domain.Services.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Scopes;

public sealed record AddScopeServiceCommand(ScopeId Scope, params ServiceId[] Services) : Command;

internal sealed class AddScopeServiceCommandHandler : CommandHandler<AddScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;

    public AddScopeServiceCommandHandler(IScopeRepository scopeRepository, IServiceRepository serviceRepository)
    {
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
    }

    public override async Task HandleAsync(AddScopeServiceCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, serviceIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var service in serviceIds)
        {
            await scope.AddServiceAsync(service, _serviceRepository);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}