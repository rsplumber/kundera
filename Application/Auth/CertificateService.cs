using Core.Domains.Scopes.Types;
using Core.Domains.Users.Types;
using Core.Extensions;
using Core.Services;
using Hashing.Abstractions;

namespace Application.Auth;

internal sealed class CertificateService : ICertificateService
{
    private readonly IHashService _hashService;

    public CertificateService(IHashService hashService)
    {
        _hashService = hashService;
    }

    public Task<Certificate> GenerateAsync(UserId user, ScopeId scope, CancellationToken cancellationToken = default)
    {
        var token = _hashService.Hash(user.Value.ToString(), scope.Value.ToString());
        var refreshToken = _hashService.Hash(new Random().RandomCharsNums(6));

        var certificate = new Certificate(token, refreshToken);
        return Task.FromResult(certificate);
    }
}