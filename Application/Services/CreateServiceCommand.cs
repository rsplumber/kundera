using Domain;
using Domain.Services;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Services;

public sealed record CreateServiceCommand(Name Name) : Command;

internal sealed class CreateServiceCommandHandler : CommandHandler<CreateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public override async Task HandleAsync(CreateServiceCommand message, CancellationToken cancellationToken = default)
    {
        var service = await Service.FromAsync(message.Name, _serviceRepository);
        await _serviceRepository.AddAsync(service, cancellationToken);
    }
}