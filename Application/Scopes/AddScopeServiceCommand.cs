using Core.Domains.Scopes;
using Core.Domains.Scopes.Exceptions;
using Core.Domains.Scopes.Types;
using Core.Domains.Services;
using Core.Domains.Services.Exceptions;
using Core.Domains.Services.Types;
using FluentValidation;
using Mediator;

namespace Application.Scopes;

public sealed record AddScopeServiceCommand : ICommand
{
    public Guid ScopeId { get; init; } = default!;

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
        var scope = await _scopeRepository.FindAsync(ScopeId.From(command.ScopeId), cancellationToken);
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
        RuleFor(request => request.ScopeId)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");

        RuleFor(request => request.Services)
            .NotEmpty().WithMessage("Enter at least one service")
            .NotNull().WithMessage("Enter at least one service");
    }
}