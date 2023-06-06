using Core.Domains.Permissions;
using Core.Domains.Services;
using Core.Domains.Services.Exceptions;
using Mediator;

namespace Application.Permissions;

public sealed record CreatePermissionCommand : ICommand<Permission>
{
    public Guid ServiceId { get; init; } = default!;

    public string Name { get; init; } = default!;

    public IDictionary<string, string>? Meta { get; init; }
}

internal sealed class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand, Permission>
{
    private readonly IServiceRepository _serviceRepository;

    public CreatePermissionCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Permission> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        var selectedService = await _serviceRepository.FindAsync(command.ServiceId, cancellationToken);
        if (selectedService is null)
        {
            throw new ServiceNotFoundException();
        }

        selectedService.AddPermission(command.Name, command.Meta);
        await _serviceRepository.UpdateAsync(selectedService, cancellationToken);
        return selectedService.Permissions.First(p => p.Name == $"{selectedService.Name}_{command.Name}");
    }
}