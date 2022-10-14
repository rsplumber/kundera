using Domain.Services;
using Domain.Services.Exceptions;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Services;

public sealed record ActivateServiceCommand(ServiceId Service) : Command;

internal sealed class ActivateServiceCommandHandler : ICommandHandler<ActivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public ActivateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async ValueTask HandleAsync(ActivateServiceCommand message, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.FindAsync(message.Service, cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        service.Activate();

        await _serviceRepository.UpdateAsync(service, cancellationToken);
    }
}