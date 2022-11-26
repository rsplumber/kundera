using FluentValidation;
using Mediator;

namespace Application.Services;

public sealed record ServiceQuery : IQuery<ServiceResponse>
{
    public Guid Service { get; init; } = default!;
}

public sealed record ServiceResponse(Guid Id, string Name, string Secret, string Status);

public sealed class ServiceQueryValidator : AbstractValidator<ServiceQuery>
{
    public ServiceQueryValidator()
    {
        RuleFor(request => request.Service)
            .NotEmpty().WithMessage("Enter a Service")
            .NotNull().WithMessage("Enter a Service");
    }
}