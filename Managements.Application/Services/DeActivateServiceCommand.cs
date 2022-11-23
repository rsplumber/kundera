using FluentValidation;
using Managements.Domain.Services;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.Services.Types;
using Mediator;

namespace Managements.Application.Services;

public sealed record DeActivateServiceCommand : ICommand
{
    public Guid Service { get; init; } = default!;
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
        var service = await _serviceRepository.FindAsync(ServiceId.From(command.Service), cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        service.DiActivate();
        await _serviceRepository.UpdateAsync(service, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DeActivateServiceCommandValidator : AbstractValidator<DeActivateServiceCommand>
{
    public DeActivateServiceCommandValidator()
    {
        RuleFor(request => request.Service)
            .NotEmpty().WithMessage("Enter a Service")
            .NotNull().WithMessage("Enter a Service");
    }
}