using System.Net;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Credentials.Exceptions;
using Mediator;

namespace Application.Auth.Credentials;

public sealed record CredentialChangePasswordCommand : ICommand
{
    public string Username { get; init; } = default!;

    public string Password { get; init; } = default!;

    public string NewPassword { get; init; } = default!;

    public string? Type { get; init; }

    public IPAddress? IpAddress { get; init; }
}

internal sealed class CredentialChangePasswordCommandHandler : ICommandHandler<CredentialChangePasswordCommand>
{
    private readonly ICredentialRepository _credentialRepository;

    public CredentialChangePasswordCommandHandler(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async ValueTask<Unit> Handle(CredentialChangePasswordCommand command, CancellationToken cancellationToken)
    {
        var credentials = await _credentialRepository.FindAsync(command.Username, cancellationToken);
        var credential = credentials.FirstOrDefault(credential => credential.Password.Check(command.Password));
        if (credential is null)
        {
            throw new CredentialNotFoundException();
        }

        credential.ChangePassword(command.Password, command.NewPassword);
        credential.UpdateActivityInfo(command.IpAddress ?? IPAddress.None);
        await _credentialRepository.UpdateAsync(credential, cancellationToken);

        return Unit.Value;
    }
}