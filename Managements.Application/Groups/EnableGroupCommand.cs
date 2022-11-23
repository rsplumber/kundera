﻿using FluentValidation;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Groups.Types;
using Mediator;

namespace Managements.Application.Groups;

public sealed record EnableGroupCommand : ICommand
{
    public Guid Group { get; set; }
}

internal sealed class EnableGroupCommandHandler : ICommandHandler<EnableGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public EnableGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(EnableGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        group.Enable();
        await _groupRepository.UpdateAsync(group, cancellationToken);

        return Unit.Value;
    }
}

public sealed class EnableGroupCommandValidator : AbstractValidator<EnableGroupCommand>
{
    public EnableGroupCommandValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");
    }
}