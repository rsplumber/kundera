using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Scopes;
using Core.Services;
using Mediator;

namespace Application.Auth.Authentications;

public sealed record RefreshCertificateCommand : ICommand<Certificate>
{
    public string Token { get; init; } = default!;

    public string RefreshToken { get; init; } = default!;

    public IPAddress? IpAddress { get; init; }
}

internal sealed class RefreshCertificateCommandHandler : ICommandHandler<RefreshCertificateCommand, Certificate>
{
    private readonly ISessionManagement _sessionManagement;
    private readonly ICredentialRepository _credentialRepository;
    private readonly IScopeRepository _scopeRepository;

    public RefreshCertificateCommandHandler(ISessionManagement sessionManagement,
        ICredentialRepository credentialRepository,
        IScopeRepository scopeRepository)
    {
        _sessionManagement = sessionManagement;
        _credentialRepository = credentialRepository;
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Certificate> Handle(RefreshCertificateCommand command, CancellationToken cancellationToken)
    {
        var token = Token.From(command.Token);
        var refreshToken = Token.From(command.RefreshToken);
        var session = await _sessionManagement.GetAsync(token, cancellationToken);
        if (session is null || session.RefreshToken != refreshToken)
        {
            throw new UnAuthorizedException();
        }

        var credential = await _credentialRepository.FindAsync(session.Credential, cancellationToken);
        if (credential is null)
        {
            throw new UnAuthorizedException();
        }

        var scope = await _scopeRepository.FindAsync(session.Scope, cancellationToken);
        if (scope is null)
        {
            throw new UnAuthorizedException();
        }

        var certificate = await _sessionManagement.SaveAsync(credential, scope, cancellationToken);
        await _sessionManagement.DeleteAsync(token, cancellationToken);

        return certificate;
    }
}