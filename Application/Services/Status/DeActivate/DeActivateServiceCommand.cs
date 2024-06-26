﻿using Core.Services;
using Core.Services.Exceptions;
using Mediator;

namespace Application.Services.Status.DeActivate;

public sealed record DeActivateServiceCommand : ICommand
{
    public Guid ServiceId { get; init; } = default!;
}

internal sealed class DeActivateServiceCommandHandler : ICommandHandler<DeActivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public DeActivateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Unit> Handle(DeActivateServiceCommand command, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.FindAsync(command.ServiceId, cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        service.DeActivate();
        await _serviceRepository.UpdateAsync(service, cancellationToken);

        return Unit.Value;
    }
}