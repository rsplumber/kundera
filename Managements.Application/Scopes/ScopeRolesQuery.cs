﻿using FluentValidation;
using Mediator;

namespace Managements.Application.Scopes;

public sealed record ScopeRolesQuery : IQuery<IEnumerable<ScopeRolesResponse>>
{
    public Guid Scope { get; init; } = default!;
}

public sealed record ScopeRolesResponse(Guid Id, string Name);

public sealed class ScopeRolesQueryValidator : AbstractValidator<ScopeRolesQuery>
{
    public ScopeRolesQueryValidator()
    {
        RuleFor(request => request.Scope)
            .NotEmpty().WithMessage("Enter a Scope")
            .NotNull().WithMessage("Enter a Scope");
    }
}