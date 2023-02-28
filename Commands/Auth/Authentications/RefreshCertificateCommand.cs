using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Scopes;
using Mediator;

namespace Commands.Auth.Authentications;

public sealed record RefreshCertificateCommand : ICommand<Certificate>
{
    public string Token { get; init; } = default!;

    public string RefreshToken { get; init; } = default!;

    public string UserAgent { get; init; } = default!;
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
        var session = await _sessionManagement.GetAsync(command.Token, cancellationToken);
        if (session is null || session.RefreshToken != command.RefreshToken)
        {
            throw new UnAuthorizedException();
        }

        if (session.UserAgent != command.UserAgent)
        {
            throw new UnAuthorizedException();
        }

        var credential = await _credentialRepository.FindAsync(session.CredentialId, cancellationToken);
        if (credential is null)
        {
            throw new UnAuthorizedException();
        }

        var scope = await _scopeRepository.FindAsync(session.ScopeId, cancellationToken);
        if (scope is null)
        {
            throw new UnAuthorizedException();
        }


        var certificate = await _sessionManagement.SaveAsync(credential, scope, command.UserAgent, cancellationToken);
        await _sessionManagement.DeleteAsync(command.Token, cancellationToken);

        return certificate;
    }
}