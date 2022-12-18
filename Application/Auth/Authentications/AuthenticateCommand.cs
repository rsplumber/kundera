using System.Net;
using Application.Auth.Certificates;
using Application.Auth.Credentials;
using Core.Domains.Auth.Credentials.Exceptions;
using Core.Domains.Scopes;
using Core.Domains.Scopes.Types;
using Core.Services;
using Mediator;

namespace Application.Auth.Authentications;

public sealed record AuthenticateCommand : ICommand<Certificate>
{
    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? Type { get; set; }

    public string ScopeSecret { get; init; } = default!;

    public IPAddress? IpAddress { get; init; }
}

internal sealed class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, Certificate>
{
    private readonly IMediator _mediator;
    private readonly ISessionManagement _sessionManagement;
    private readonly IScopeRepository _scopeRepository;


    public AuthenticateCommandHandler(IMediator mediator, ISessionManagement sessionManagement, IScopeRepository scopeRepository)
    {
        _mediator = mediator;
        _sessionManagement = sessionManagement;
        _scopeRepository = scopeRepository;
    }

    public async ValueTask<Certificate> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
    {
        var credential = await _mediator.Send(new ValidateCredentialCommand
        {
            Username = command.Username,
            Type = command.Type
        }, cancellationToken);

        var scope = await _scopeRepository.FindAsync(ScopeSecret.From(command.ScopeSecret), cancellationToken);
        if (credential is null || scope is null)
        {
            throw new UnAuthenticateException();
        }

        try
        {
            credential.Password.Check(command.Password);
        }
        catch
        {
            throw new UnAuthenticateException();
        }

        var certificate = await _mediator.Send(new GenerateCertificateCommand
        {
            UserId = credential.User.Value,
            ScopeId = scope.Id.Value
        }, cancellationToken);

        await _sessionManagement.SaveAsync(certificate, credential.User, scope.Id, cancellationToken);

        return certificate;
    }
}