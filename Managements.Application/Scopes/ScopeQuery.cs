using FluentValidation;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record ScopeQuery : IQuery<ScopeResponse>
{
    public Guid Scope { get; init; } = default!;
}

public sealed record ScopeResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Secret { get; set; }

    public string Status { get; set; }

    public IEnumerable<Guid>? Roles { get; set; }

    public IEnumerable<Guid>? Services { get; set; }
}

public sealed class ScopeQueryValidator : AbstractValidator<ScopeQuery>
{
    public ScopeQueryValidator()
    {
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");
    }
}