using System.Net;
using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Sessions;
using Mediator;

namespace Application.Auth.Authentications;

public sealed record RefreshCertificateCommand : ICommand<Certificate>
{
    public string Token { get; init; } = default!;

    public string RefreshToken { get; init; } = default!;

    public string UserAgent { get; init; } = default!;

    public IPAddress IpAddress { get; init; } = default!;
}

internal sealed class RefreshCertificateCommandHandler : ICommandHandler<RefreshCertificateCommand, Certificate>
{
    private readonly ISessionManagement _sessionManagement;

    public RefreshCertificateCommandHandler(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    public async ValueTask<Certificate> Handle(RefreshCertificateCommand command, CancellationToken cancellationToken)
    {
        var session = await _sessionManagement.GetAsync(command.Token, cancellationToken);
        if (session is null || session.RefreshToken != command.RefreshToken)
        {
            throw new UnAuthorizedException();
        }

        var certificate = await _sessionManagement.SaveAsync(session.Credential, session.Scope, command.IpAddress, command.UserAgent, cancellationToken);
        await _sessionManagement.DeleteAsync(command.Token, cancellationToken);

        return certificate;
    }
}