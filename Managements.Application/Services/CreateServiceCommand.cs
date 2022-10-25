using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Services;

namespace Managements.Application.Services;

public sealed record CreateServiceCommand(Name Name) : Command;

internal sealed class CreateServiceCommandHandler : ICommandHandler<CreateServiceCommand>
{
    private readonly IServiceFactory _serviceFactory;
    private readonly IServiceRepository _serviceRepository;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IServiceFactory serviceFactory)
    {
        _serviceRepository = serviceRepository;
        _serviceFactory = serviceFactory;
    }

    public async Task HandleAsync(CreateServiceCommand message, CancellationToken cancellationToken = default)
    {
        var service = await _serviceFactory.CreateAsync(message.Name);
        await _serviceRepository.AddAsync(service, cancellationToken);
    }
}