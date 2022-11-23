using FluentValidation;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Types;
using Managements.Domain.Roles.Types;
using Mediator;

namespace Managements.Application.Groups;

public sealed record CreateGroupCommand : ICommand<Group>
{
    public string Name { get; init; } = default!;

    public Guid Role { get; init; } = default!;

    public Guid? Parent { get; init; }
}

internal sealed class CreateGroupCommandHandler : ICommandHandler<CreateGroupCommand, Group>
{
    private readonly IGroupFactory _groupFactory;

    public CreateGroupCommandHandler(IGroupFactory groupFactory)
    {
        _groupFactory = groupFactory;
    }

    public async ValueTask<Group> Handle(CreateGroupCommand command, CancellationToken cancellationToken)
    {
        var group = await _groupFactory.CreateAsync(command.Name,
            RoleId.From(command.Role),
            command.Parent is not null ? GroupId.From(command.Parent.Value) : null);

        return group;
    }
}

public sealed class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter Name")
            .NotNull().WithMessage("Enter Name");

        RuleFor(request => request.Role)
            .NotEmpty().WithMessage("Enter Name")
            .NotNull().WithMessage("Enter Name");
    }
}