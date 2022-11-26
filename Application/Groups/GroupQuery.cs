﻿using FluentValidation;
using Mediator;

namespace Application.Groups;

public sealed record GroupQuery : IQuery<GroupResponse>
{
    public Guid Id { get; set; }
}

public sealed record GroupResponse(Guid Id, string Name, string Status)
{
    public string? Description { get; set; }

    public Guid? Parent { get; set; }

    public DateTime? StatusChangedDate { get; set; }

    public IEnumerable<GroupRoleResponse> Roles { get; set; }
}

public sealed record GroupRoleResponse(Guid Id, string Name);

public sealed class GroupQueryValidator : AbstractValidator<GroupQuery>
{
    public GroupQueryValidator()
    {
        RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Enter a group id")
            .NotNull().WithMessage("Enter a group id");
    }
}