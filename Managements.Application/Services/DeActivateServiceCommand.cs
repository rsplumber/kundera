using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain.Services;
using Managements.Domain.Services.Exceptions;

namespace Managements.Application.Services;

public sealed record DeActivateServiceCommand(ServiceId Service) : Command;

internal sealed class DeActivateServiceCommandHandler : ICommandHandler<DeActivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public DeActivateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task HandleAsync(DeActivateServiceCommand message, CancellationToken cancellationToken = default)
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