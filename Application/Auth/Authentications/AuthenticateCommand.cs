using Core.Domains.Auth.Authorizations;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Credentials.Exceptions;
using Core.Domains.Auth.Sessions;
using Core.Domains.Scopes;
using Mediator;

namespace Application.Auth.Authentications;

public sealed record AuthenticateCommand : ICommand<Certificate>
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string ScopeSecret { get; init; } = default!;

    public string UserAgent { get; init; } = default!;
}

internal sealed class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, Certificate>
{
    private readonly ISessionManagement _sessionManagement;
    private readonly IScopeRepository _scopeRepository;
    private readonly ICredentialRepository _credentialRepository;
    private readonly ISessionRepository _sessionRepository;


    public AuthenticateCommandHandler(ISessionManagement sessionManagement,
        IScopeRepository scopeRepository,
        ICredentialRepository credentialRepository,
        ISessionRepository sessionRepository)
    {
        _sessionManagement = sessionManagement;
        _scopeRepository = scopeRepository;
        _credentialRepository = credentialRepository;
        _sessionRepository = sessionRepository;
    }

    public async ValueTask<Certificate> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var credentials = await _credentialRepository.FindByUsernameAsync(command.Username, cancellationToken);
        var credential = credentials.SingleOrDefault(credential => credential.Username == command.Username && credential.Password.Check(command.Password));
        var scope = await _scopeRepository.FindBySecretAsync(command.ScopeSecret, cancellationToken);
        if (credential is null || scope is null)
        {
            throw new WrongUsernamePasswordException();
        }

        if (CredentialExpired())
        {
            await _credentialRepository.DeleteAsync(credential.Id, cancellationToken);
            throw new WrongUsernamePasswordException();
        }

        if (credential.OneTime)
        {
            await _credentialRepository.DeleteAsync(credential.Id, cancellationToken);
        }

        if (!credential.Password.Check(command.Password))
        {
            throw new WrongUsernamePasswordException();
        }

        if (credential.SingleSession)
        {
            var currentCredentialSessions = await _sessionRepository.FindByCredentialIdAsync(credential.Id, cancellationToken);
            foreach (var currentCredentialSession in currentCredentialSessions)
            {
                await _sessionRepository.DeleteAsync(currentCredentialSession.Id, cancellationToken);
            }
        }

        return await _sessionManagement.SaveAsync(credential, scope, command.UserAgent, cancellationToken);

        bool CredentialExpired() => DateTime.UtcNow >= credential.ExpiresAtUtc;
    }
}