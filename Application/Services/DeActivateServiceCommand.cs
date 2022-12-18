using Core.Domains.Services;
using Core.Domains.Services.Exceptions;
using Core.Domains.Services.Types;
using Mediator;

namespace Application.Services;

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
        var service = await _serviceRepository.FindAsync(ServiceId.From(command.ServiceId), cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        service.DiActivate();
        await _serviceRepository.UpdateAsync(service, cancellationToken);

        return Unit.Value;
    }
}