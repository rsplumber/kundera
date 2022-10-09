﻿using Domain.Services;
using Domain.Services.Exceptions;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Services;

public sealed record DeleteServiceCommand(ServiceId Id) : Command;

internal sealed class DeleteServiceCommandHandler : CommandHandler<DeleteServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public DeleteServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public override async Task HandleAsync(DeleteServiceCommand message, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.FindAsync(message.Id, cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        await _serviceRepository.DeleteAsync(service.Id, cancellationToken);
    }
}