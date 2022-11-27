using FluentValidation;
using Mediator;

namespace Application.Scopes;

public sealed record ScopeQuery : IQuery<ScopeResponse>
{
    public Guid Scope { get; init; } = default!;
}

public sealed record ScopeResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; }  = string.Empty;

    public string Secret { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public IEnumerable<Guid>? Roles { get; init; }

    public IEnumerable<Guid>? Services { get; init; }
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