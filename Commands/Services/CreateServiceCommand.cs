using Core.Domains.Services;
using Mediator;

namespace Commands.Services;

public sealed record CreateServiceCommand : ICommand<Service>
{
    public string Name { get; init; } = default!;
}

internal sealed class CreateServiceCommandHandler : ICommandHandler<CreateServiceCommand, Service>
{
    private readonly IServiceFactory _serviceFactory;

    public CreateServiceCommandHandler(IServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public async ValueTask<Service> Handle(CreateServiceCommand command, CancellationToken cancellationToken)
    {
        var service = await _serviceFactory.CreateAsync(command.Name);
        return service;
    }
}