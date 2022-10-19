using Kite.CQRS;
using Kite.CQRS.Contracts;
using Kite.Hashing;
using Managements.Domain;
using Managements.Domain.Services;

namespace Managements.Application.Services;

public sealed record CreateServiceCommand(Name Name) : Command;

internal sealed class CreateServiceCommandHandler : ICommandHandler<CreateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IHashService _hashService;

    public CreateServiceCommandHandler(IServiceRepository serviceRepository, IHashService hashService)
    {
        _serviceRepository = serviceRepository;
        _hashService = hashService;
    }

    public async Task HandleAsync(CreateServiceCommand message, CancellationToken cancellationToken = default)
    {
        var service = await Service.FromAsync(message.Name, _hashService, _serviceRepository);
        await _serviceRepository.AddAsync(service, cancellationToken);
    }
}