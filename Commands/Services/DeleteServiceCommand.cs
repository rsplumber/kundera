using Core.Domains.Services;
using Core.Domains.Services.Exceptions;
using Mediator;

namespace Commands.Services;

public sealed record DeleteServiceCommand : ICommand
{
    public Guid ServiceId { get; init; } = default!;
}

internal sealed class DeleteServiceCommandHandler : ICommandHandler<DeleteServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public DeleteServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Unit> Handle(DeleteServiceCommand command, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.FindAsync(command.ServiceId, cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        await _serviceRepository.DeleteAsync(service.Id, cancellationToken);

        return Unit.Value;
    }
}