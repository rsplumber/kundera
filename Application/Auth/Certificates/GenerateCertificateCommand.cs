using Core.Extensions;
using Core.Hashing;
using Core.Services;
using Mediator;

namespace Application.Auth.Certificates;

public sealed record GenerateCertificateCommand : ICommand<Certificate>
{
    public Guid UserId { get; init; } = default!;

    public Guid ScopeId { get; init; } = default!;
}

internal sealed class GenerateCertificateCommandHandler : ICommandHandler<GenerateCertificateCommand, Certificate>
{
    private readonly IHashService _hashService;

    public GenerateCertificateCommandHandler(IHashService hashService)
    {
        _hashService = hashService;
    }

    public ValueTask<Certificate> Handle(GenerateCertificateCommand command, CancellationToken cancellationToken)
    {
        var token = _hashService.Hash(command.UserId.ToString(), command.ScopeId.ToString());
        var refreshToken = _hashService.Hash(new Random().RandomCharsNums(6));
        var certificate = new Certificate(token, refreshToken);
        return ValueTask.FromResult(certificate);
    }
}