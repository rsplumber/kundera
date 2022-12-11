using FluentValidation;
using Mediator;

namespace Application.Services;

public sealed record ServiceQuery : IQuery<ServiceResponse>
{
    public Guid ServiceId { get; init; } = default!;
}

public sealed record ServiceResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Secret { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;
};

public sealed class ServiceQueryValidator : AbstractValidator<ServiceQuery>
{
    public ServiceQueryValidator()
    {
        RuleFor(request => request.ServiceId)
            .NotEmpty().WithMessage("Enter a Service")
            .NotNull().WithMessage("Enter a Service");
    }
}