using Domain.Users;
using Tes.CQRS;
using Tes.CQRS.Contracts;

namespace Application.Users;

public sealed record ExistUserUsernameCommand(Username Username) : Command;

internal sealed class ExistUserUsernameCommandHandler : CommandHandler<ExistUserUsernameCommand>
{
    private readonly IUserRepository _userRepository;

    public ExistUserUsernameCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override async Task<bool> HandleAsync(ExistUserUsernameCommand message, CancellationToken cancellationToken = default)
    {
        return await _userRepository.ExistsAsync(message.Username, cancellationToken);
    }
}