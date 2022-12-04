using System.Net;
using Application.Auth.Certificates;
using Core.Domains.Auth.Credentials.Exceptions;
using Core.Domains.Auth.Sessions;
using Core.Services;
using FluentValidation;
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
    private readonly IMediator _mediator;
    private readonly ISessionManagement _sessionManagement;

    public RefreshCertificateCommandHandler(IMediator mediator, ISessionManagement sessionManagement)
    {
        _mediator = mediator;
        _sessionManagement = sessionManagement;
    }

    public async ValueTask<Certificate> Handle(RefreshCertificateCommand command, CancellationToken cancellationToken)
    {
        var token = Token.From(command.Token);
        var refreshToken = Token.From(command.RefreshToken);
        var session = await _sessionManagement.GetAsync(token, cancellationToken);
        if (session is null || session.RefreshToken != refreshToken)
        {
            throw new UnAuthenticateException();
        }

        var userId = session.User;
        var scopeId = session.Scope;
        var certificate = await _mediator.Send(new GenerateCertificateCommand
        {
            UserId = userId.Value,
            ScopeId = scopeId.Value
        }, cancellationToken);

        await _sessionManagement.SaveAsync(certificate, userId, scopeId, cancellationToken);
        await _sessionManagement.DeleteAsync(token, cancellationToken);

        return certificate;
    }
}

public sealed class RefreshCertificateCommandValidator : AbstractValidator<RefreshCertificateCommand>
{
    public RefreshCertificateCommandValidator()
    {
        RuleFor(request => request.Token)
            .NotEmpty().WithMessage("Enter valid Token")
            .NotNull().WithMessage("Enter valid Token");

        RuleFor(request => request.RefreshToken)
            .NotEmpty().WithMessage("Enter valid RefreshToken")
            .NotNull().WithMessage("Enter valid RefreshToken");
    }
}