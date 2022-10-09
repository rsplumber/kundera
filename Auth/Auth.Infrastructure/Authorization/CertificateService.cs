using Auth.Application;
using Auth.Application.Authorization;
using Tes.Standard.Tokens;
using Token = Auth.Domain.Sessions.Token;

namespace Authentication.Infrastructure.Authorization;

internal sealed class CertificateService : ICertificateService
{
    private readonly ITokenService _tokenService;

    public CertificateService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }


    public async ValueTask<Certificate> GenerateAsync(string id, string scope = "global", CancellationToken cancellationToken = default)
    {
        var tokenProperties = new TokenProperties();
        tokenProperties.Add("id", id);
        tokenProperties.Add("scope", scope);
        var token = await _tokenService.GenerateAsync(tokenProperties);
        var refreshToken = await _tokenService.GenerateAsync(new TokenProperties());
        return new Certificate(Token.From(token.Value), Token.From(refreshToken.Value));
    }
}