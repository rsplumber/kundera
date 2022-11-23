using FluentValidation;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record ScopeQuery : IQuery<ScopeResponse>
{
    public Guid Scope { get; init; } = default!;
}

public sealed record ScopeResponse(Guid Id, string Name, string Secret, string Status)
{
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