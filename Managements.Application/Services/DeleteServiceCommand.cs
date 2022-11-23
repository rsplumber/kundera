using FluentValidation;
using Managements.Domain.Services;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.Services.Types;
using Mediator;

namespace Managements.Application.Services;

public sealed record DeleteServiceCommand : ICommand
{
    public Guid Service { get; init; } = default!;
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
        var service = await _serviceRepository.FindAsync(ServiceId.From(command.Service), cancellationToken);
        if (service is null)
        {
            throw new ServiceNotFoundException();
        }

        await _serviceRepository.DeleteAsync(service.Id, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DeleteServiceCommandValidator : AbstractValidator<DeleteServiceCommand>
{
    public DeleteServiceCommandValidator()
    {
        RuleFor(request => request.Service)
            .NotEmpty().WithMessage("Enter a Service")
            .NotNull().WithMessage("Enter a Service");
    }
}