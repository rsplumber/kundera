using Domain.Services;
using Domain.Services.Exceptions;
using Kite.CQRS;
using Kite.CQRS.Contracts;

namespace Application.Services;

public sealed record DeActivateServiceCommand(ServiceId Service) : Command;

internal sealed class DeActivateServiceCommandHandler : ICommandHandler<DeActivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public DeActivateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async ValueTask HandleAsync(DeActivateServiceCommand message, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.FindAsync(message.Service, cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        service.DiActivate();
        await _serviceRepository.UpdateAsync(service, cancellationToken);
    }
}