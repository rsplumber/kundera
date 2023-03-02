using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Core.Domains.Services;
using Core.Domains.Services.Exceptions;
using Mediator;

namespace Application.Scopes;

public sealed record AddScopeServiceCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

    public Guid[] Services { get; init; } = default!;
}

internal sealed class AddScopeServiceCommandHandler : ICommandHandler<AddScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;

    public AddScopeServiceCommandHandler(IScopeRepository scopeRepository, IServiceRepository serviceRepository)
    {
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Unit> Handle(AddScopeServiceCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(command.ScopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var serviceId in command.Services)
        {
            var service = await _serviceRepository.FindAsync(serviceId, cancellationToken);
            if (service is null)
            {
                throw new ServiceNotFoundException();
            }

            scope.Add(service);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}