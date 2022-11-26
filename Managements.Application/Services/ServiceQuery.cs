using FluentValidation;
using Mediator;

namespace Managements.Application.Services;

public sealed record ServiceQuery : IQuery<ServiceResponse>
{
    public Guid Service { get; init; } = default!;
}

public sealed record ServiceResponse
{
    public Guid Id { set; get; }

    public string Name { set; get; }

    public string Secret { set; get; }

    public string Status { set; get; }
};

public sealed class ServiceQueryValidator : AbstractValidator<ServiceQuery>
{
    public ServiceQueryValidator()
    {
        RuleFor(request => request.Service)
            .NotEmpty().WithMessage("Enter a Service")
            .NotNull().WithMessage("Enter a Service");
    }
}