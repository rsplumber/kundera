using FluentValidation;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Groups.Types;
using Mediator;

namespace Managements.Application.Groups;

public sealed record DeleteGroupCommand : ICommand
{
    public Guid Group { get; init; } = default!;
}

internal sealed class DeleteGroupCommandHandler : ICommandHandler<DeleteGroupCommand>
{
    private readonly IGroupRepository _groupRepository;

    public DeleteGroupCommandHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }


    public async ValueTask<Unit> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteAsync(group.Id, cancellationToken);

        return Unit.Value;
    }
}

public sealed class DeleteGroupCommandValidator : AbstractValidator<DeleteGroupCommand>
{
    public DeleteGroupCommandValidator()
    {
        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter a Group")
            .NotNull().WithMessage("Enter a Group");
    }
}