using Core.Services;

namespace Application.Auth;

internal sealed class CertificateService : ICertificateService
{
    private readonly IHashService _hashService;

    public CertificateService(IHashService hashService)
    {
        _hashService = hashService;
    }

    public Task<Certificate> GenerateAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
    {
        var token = _hashService.Hash(userId.ToString(), scopeId.ToString());
        var refreshToken = _hashService.Hash(new Random().RandomCharsNums(6));

        var certificate = new Certificate(token, refreshToken);
        return Task.FromResult(certificate);
    }
}