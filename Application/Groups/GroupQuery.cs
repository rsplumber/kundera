﻿using FluentValidation;
using Mediator;

namespace Application.Groups;

public sealed record GroupQuery : IQuery<GroupResponse>
{
    public Guid GroupId { get; init; }
}

public sealed record GroupResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Guid? Parent { get; init; }

    public DateTime? StatusChangedDate { get; init; }

    public IEnumerable<GroupRoleResponse> Roles { get; init; } = Enumerable.Empty<GroupRoleResponse>();
}

public sealed record GroupRoleResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
}

public sealed class GroupQueryValidator : AbstractValidator<GroupQuery>
{
    public GroupQueryValidator()
    {
        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter a group id")
            .NotNull().WithMessage("Enter a group id");
    }
}