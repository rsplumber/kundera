using Kite.CQRS;
using Kite.CQRS.Contracts;
using Managements.Domain;
using Managements.Domain.Services;

namespace Managements.Application.Services;

public sealed record CreateServiceCommand(Name Name) : Command;

internal sealed class CreateServiceCommandHandler : ICommandHandler<CreateServiceCommand>
{
    private readonly IServiceFactory _serviceFactory;

    public CreateServiceCommandHandler(IServiceFactory serviceFactory)
    {
        _serviceFactory = serviceFactory;
    }

    public async Task HandleAsync(CreateServiceCommand message, CancellationToken cancellationToken = default)
    {
        await _serviceFactory.CreateAsync(message.Name);
    }
}