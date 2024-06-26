﻿using Core.Scopes;
using Core.Scopes.Exceptions;
using Core.Services;
using Core.Services.Exceptions;
using Mediator;

namespace Application.Scopes.Services.Delete;

public sealed record DeleteScopeServiceCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

    public Guid[] ServicesIds { get; init; } = default!;
}

internal sealed class DeleteScopeServiceCommandHandler : ICommandHandler<DeleteScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;

    public DeleteScopeServiceCommandHandler(IScopeRepository scopeRepository, IServiceRepository serviceRepository)
    {
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Unit> Handle(DeleteScopeServiceCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(command.ScopeId, cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var serviceId in command.ServicesIds)
        {
            var service = await _serviceRepository.FindAsync(serviceId, cancellationToken);
            if (service is null)
            {
                throw new ServiceNotFoundException();
            }
            scope.Remove(service);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);

        return Unit.Value;
    }
}