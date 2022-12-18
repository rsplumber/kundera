﻿using Core.Domains.Services;
using Core.Domains.Services.Exceptions;
using Core.Domains.Services.Types;
using Mediator;

namespace Application.Services;

public sealed record ActivateServiceCommand : ICommand
{
    public Guid ServiceId { get; init; } = default!;
}

internal sealed class ActivateServiceCommandHandler : ICommandHandler<ActivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public ActivateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Unit> Handle(ActivateServiceCommand command, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.FindAsync(ServiceId.From(command.ServiceId), cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        service.Activate();

        await _serviceRepository.UpdateAsync(service, cancellationToken);

        return Unit.Value;
    }
}