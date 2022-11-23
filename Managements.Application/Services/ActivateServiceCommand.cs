using FluentValidation;
using Managements.Domain.Services;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.Services.Types;
using Mediator;

namespace Managements.Application.Services;

public sealed record ActivateServiceCommand : ICommand
{
    public Guid Service { get; init; } = default!;
}

internal sealed class ActivateServiceCommandHandler : ICommandHandler<ActivateServiceCommand>
{
    private readonly IServiceRepository _serviceRepository;

    public ActivateServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Unit> Handle(ActivateServiceCommand command, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.FindAsync(ServiceId.From(command.Service), cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        service.Activate();

        await _serviceRepository.UpdateAsync(service, cancellationToken);

        return Unit.Value;
    }
}

public sealed class ActivateServiceCommandValidator : AbstractValidator<ActivateServiceCommand>
{
    public ActivateServiceCommandValidator()
    {
        RuleFor(request => request.Service)
            .NotEmpty().WithMessage("Enter a Service")
            .NotNull().WithMessage("Enter a Service");
    }
}