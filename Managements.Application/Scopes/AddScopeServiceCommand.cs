using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Services;
using Managements.Domain.Services.Exceptions;

namespace Managements.Application.Scopes;

public sealed record AddScopeServiceCommand(ScopeId Scope, params ServiceId[] Services) : Command;

internal sealed class AddScopeServiceCommandHandler : ICommandHandler<AddScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;

    public AddScopeServiceCommandHandler(IScopeRepository scopeRepository, IServiceRepository serviceRepository)
    {
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task HandleAsync(AddScopeServiceCommand message, CancellationToken cancellationToken = default)
    {
        var (scopeId, serviceIds) = message;
        var scope = await _scopeRepository.FindAsync(scopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var serviceId in serviceIds)
        {
            var service = await _serviceRepository.FindAsync(serviceId, cancellationToken);
            if (service is null)
            {
                throw new ServiceNotFoundException();
            }

            scope.AddService(service.Id);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
    }
}