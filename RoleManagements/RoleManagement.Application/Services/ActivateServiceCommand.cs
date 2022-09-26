using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Exceptions;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace RoleManagement.Application.Services;

public sealed record ActivateServiceCommand(ServiceId Service) : Command;

internal sealed class ActivateServiceCommandHandler : CommandHandler<ActivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public ActivateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public override async Task HandleAsync(ActivateServiceCommand message, CancellationToken cancellationToken = default)
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