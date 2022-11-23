using FluentValidation;
using Managements.Domain.Users;
using Managements.Domain.Users.Exception;
using Managements.Domain.Users.Types;
using Mediator;

namespace Managements.Application.Users;

public sealed record BlockUserCommand : ICommand
{
    public Guid User { get; init; } = default!;

    public string Reason { get; init; } = default!;
}

internal sealed class BlockUserCommandHandler : ICommandHandler<BlockUserCommand>
{
    private readonly IUserRepository _userRepository;

    public BlockUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<Unit> Handle(BlockUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindAsync(UserId.From(command.User), cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException();
        }

        user.Block(command.Reason!);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Unit.Value;
    }
}

public sealed class BlockUserCommandValidator : AbstractValidator<BlockUserCommand>
{
    public BlockUserCommandValidator()
    {
        RuleFor(request => request.User)
            .NotEmpty().WithMessage("Enter User")
            .NotNull().WithMessage("Enter User");

        RuleFor(request => request.Reason)
            .NotEmpty().WithMessage("Enter Reason")
            .NotNull().WithMessage("Enter Reason");
    }
}