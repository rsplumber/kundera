using Core.Permissions;
using Core.Services;
using Core.Services.Exceptions;
using Mediator;

namespace Application.Services.Permissions.Create;

public sealed record CreatePermissionCommand : ICommand<Permission>
{
    public Guid ServiceId { get; init; } = default!;

    public string Name { get; init; } = default!;

    public IDictionary<string, string>? Meta { get; init; }
}

internal sealed class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand, Permission>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IPermissionService _permissionService;

    public CreatePermissionCommandHandler(IServiceRepository serviceRepository, IPermissionService permissionService)
    {
        _serviceRepository = serviceRepository;
        _permissionService = permissionService;
    }

    public async ValueTask<Permission> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
    {
        var selectedService = await _serviceRepository.FindAsync(command.ServiceId, cancellationToken);
        if (selectedService is null)
        {
            throw new ServiceNotFoundException();
        }

        var permission = await _permissionService.CreateAsync(selectedService, command.Name, command.Meta, cancellationToken);
        return permission;
    }
}