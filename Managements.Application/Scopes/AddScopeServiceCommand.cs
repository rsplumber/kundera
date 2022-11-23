using FluentValidation;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Exceptions;
using Managements.Domain.Scopes.Types;
using Managements.Domain.Services;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.Services.Types;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record AddScopeServiceCommand : ICommand
{
    public Guid Scope { get; init; } = default!;

    public Guid[] Services { get; init; } = default!;
}

internal sealed class AddScopeServiceCommandHandler : ICommandHandler<AddScopeServiceCommand>
{
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;

    public AddScopeServiceCommandHandler(IScopeRepository scopeRepository, IServiceRepository serviceRepository)
    {
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
    }

    public async ValueTask<Unit> Handle(AddScopeServiceCommand command, CancellationToken cancellationToken)
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.Scope), cancellationToken);
        if (scope is null)
        {
            throw new ScopeNotFoundException();
        }

        foreach (var serviceId in command.Services)
        {
            var service = await _serviceRepository.FindAsync(ServiceId.From(serviceId), cancellationToken);
            if (service is null)
            {
                throw new ServiceNotFoundException();
            }

            scope.AddService(service.Id);
        }

        await _scopeRepository.UpdateAsync(scope, cancellationToken);
        
        return Unit.Value;
    }
}

public sealed class AddScopeServiceCommandValidator : AbstractValidator<AddScopeServiceCommand>
{
    public AddScopeServiceCommandValidator()
    {
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");

        RuleFor(request => request.Services)
            .NotEmpty().WithMessage("Enter at least one service")
            .NotNull().WithMessage("Enter at least one service");
    }
}