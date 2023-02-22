using Core.Domains.Services;
using Core.Domains.Services.Exceptions;
using Mediator;

namespace Commands.Services;

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

        service.DiActivate();
        await _serviceRepository.UpdateAsync(service, cancellationToken);

        return Unit.Value;
    }
}