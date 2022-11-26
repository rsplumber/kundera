using Core.Domains.Services;
using FluentValidation;
using Mediator;

namespace Application.Services;

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

public sealed class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");
    }
}