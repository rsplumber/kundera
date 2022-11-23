﻿using FluentValidation;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Groups.Types;
using Mediator;

namespace Managements.Application.Groups;

public sealed record SetGroupParentCommand : ICommand
{
    public Guid Group { get; init; } = default!;

    public Guid Parent { get; init; } = default!;
}

internal sealed class SetGroupParentCommandHandler : ICommandHandler<SetGroupParentCommand>
{
    private readonly IGroupRepository _groupRepository;

    public SetGroupParentCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(SetGroupParentCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        var parent = await _groupRepository.FindAsync(GroupId.From(command.Parent), cancellationToken);
        if (parent is null)
        {
            throw new GroupNotFoundException();
        }

        group.SetParent(parent.Id);
        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}

public sealed class SetGroupParentCommandValidator : AbstractValidator<SetGroupParentCommand>
{
    public SetGroupParentCommandValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");

        RuleFor(request => request.Parent)
            .NotEmpty().WithMessage("Enter a at least one role")
            .NotNull().WithMessage("Enter a at least one role");
    }
}