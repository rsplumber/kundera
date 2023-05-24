using System.Net;
using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using DotNetCore.CAP;
using Mediator;

namespace Application.Auth.Authentications;

public sealed record RefreshCertificateCommand : ICommand<Certificate>
{
    public string Token { get; init; } = default!;

    public string RefreshToken { get; init; } = default!;

    public string? UserAgent { get; init; }

    public IPAddress IpAddress { get; init; } = default!;
}

internal sealed class RefreshCertificateCommandHandler : ICommandHandler<RefreshCertificateCommand, Certificate>
{
    private readonly ISessionManagement _sessionManagement;
    private readonly ICapPublisher _eventBus;
    private const string RefreshedKey = "$$refreshed_token$$";


    public RefreshCertificateCommandHandler(ISessionManagement sessionManagement, ICapPublisher eventBus)
    {
        _sessionManagement = sessionManagement;
        _eventBus = eventBus;
    }

    public async ValueTask<Certificate> Handle(RefreshCertificateCommand command, CancellationToken cancellationToken)
    {
        var session = await _sessionManagement.GetAsync(command.Token, cancellationToken);
        if (session is null || session.RefreshToken != command.RefreshToken)
        {
            throw new UnAuthorizedException();
        }

        var certificate = await _sessionManagement.SaveAsync(session.Credential, session.Scope, cancellationToken);
        await _sessionManagement.DeleteAsync(command.Token, cancellationToken);

        Task.Run(() =>
        {
            _eventBus.PublishAsync(AuthenticatedEvent.EventName, new AuthenticatedEvent
            {
                Agent = command.UserAgent,
                IpAddress = command.IpAddress.ToString(),
                Username = RefreshedKey,
                CredentialId = session.Credential.Id,
                UserId = session.User.Id,
                ScopeId = session.Scope.Id
            }, cancellationToken: cancellationToken);
        }, cancellationToken);

        return certificate;
    }
}