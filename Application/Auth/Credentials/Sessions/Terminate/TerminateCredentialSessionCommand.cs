using Core.Auth.Sessions;
using Mediator;

namespace Application.Auth.Credentials.Sessions.Terminate;

public sealed record TerminateCredentialSessionCommand : ICommand
{
    public Guid Id { get; set; } = default!;
}

internal sealed class TerminateCredentialSessionCommandHandler : ICommandHandler<TerminateCredentialSessionCommand>
{
    private readonly ISessionRepository _sessionRepository;

    public TerminateCredentialSessionCommandHandler(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async ValueTask<Unit> Handle(TerminateCredentialSessionCommand command, CancellationToken cancellationToken)
    {
        var sessions = await _sessionRepository.FindByCredentialIdAsync(command.Id, cancellationToken);
        foreach (var session in sessions)
        {
            await _sessionRepository.DeleteAsync(session.Id, cancellationToken);
        }

        return Unit.Value;
    }
}