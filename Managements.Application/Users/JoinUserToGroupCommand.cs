using FluentValidation;
using Managements.Domain.Groups;
using Managements.Domain.Groups.Exception;
using Managements.Domain.Groups.Types;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;
using Mediator;

namespace Managements.Application.Users;

public sealed record JoinUserToGroupCommand : ICommand
{
    public Guid User { get; init; } = default!;

    public Guid Group { get; init; } = default!;
}

internal sealed class JoinUserToGroupCommandHandler : ICommandHandler<JoinUserToGroupCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;

    public JoinUserToGroupCommandHandler(IUserRepository userRepository, IGroupRepository groupRepository)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }

    public async ValueTask<Unit> Handle(JoinUserToGroupCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.User), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var group = await _groupRepository.FindAsync(GroupId.From(command.Group), cancellationToken);
        if (group is null)
        {
            throw new GroupNotFoundException();
        }

        user.JoinGroup(group.Id);
        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}

public sealed class JoinUserToGroupCommandValidator : AbstractValidator<JoinUserToGroupCommand>
{
    public JoinUserToGroupCommandValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter User")
            .NotNull().WithMessage("Enter User");

        RuleFor(request => request.Group)
            .NotEmpty().WithMessage("Enter Group")
            .NotNull().WithMessage("Enter Group");
    }
}