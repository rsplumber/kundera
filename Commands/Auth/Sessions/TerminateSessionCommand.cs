using Core.Domains.Auth.Sessions;
using Mediator;

namespace Commands.Auth.Sessions;

public sealed record TerminateSessionCommand : ICommand
{
    public string Id { get; init; } = default!;
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
        var session = await _sessionManagement.GetAsync(command.Id, cancellationToken);
        if (session is null)
        {
            throw new SessionNotFoundException();
        }

        await _sessionManagement.DeleteAsync(session.Id, cancellationToken);
        return Unit.Value;
    }
}