using Core.Auth.Sessions;
using Mediator;

namespace Application.Auth.Sessions.Terminate;

public sealed record TerminateSessionCommand : ICommand
{
    public string Token { get; init; } = default!;
}

internal sealed class TerminateSessionCommandHandler : ICommandHandler<TerminateSessionCommand>
{
    private readonly ISessionManagement _sessionManagement;

    public TerminateSessionCommandHandler(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public async ValueTask<Unit> Handle(TerminateSessionCommand command, CancellationToken cancellationToken)
    {
        var session = await _sessionManagement.GetAsync(command.Token, cancellationToken);
        if (session is null)
        {
            throw new SessionNotFoundException();
        }

        await _sessionManagement.DeleteAsync(session.Id, cancellationToken);
        return Unit.Value;
    }
}