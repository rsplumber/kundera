using Core.Auth.Sessions;
using Mediator;

namespace Application.Auth.Logout;

public class LogoutCommand : ICommand
{
    public string RefreshToken { get; init; } = default!;
}

internal sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly ISessionManagement _sessionManagement;

    public LogoutCommandHandler(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public async ValueTask<Unit> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var currentSession = await _sessionManagement.GetByRefreshTokenAsync(command.RefreshToken, cancellationToken);
        if (currentSession is null)
        {
            throw new SessionNotFoundException();
        }

        await _sessionManagement.DeleteAsync(currentSession.Id, cancellationToken);
        return Unit.Value;
    }
}