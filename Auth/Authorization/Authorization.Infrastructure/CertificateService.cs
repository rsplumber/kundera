using Authorization.Application;
using Tes.Standard.Tokens;
using Token = Authorization.Domain.Types.Token;

namespace Authorization.Infrastructure;

internal sealed class CertificateService : ICertificateService
{
    
    private readonly ITokenService _tokenService;

    public CertificateService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }


    public async Task<Certificate> GenerateAsync(string id, CancellationToken cancellationToken = default)
    {
        var tokenProperties = new TokenProperties();
        tokenProperties.Add("id", id);
        var token = await _tokenService.GenerateAsync(tokenProperties);
        var refreshToken = await _tokenService.GenerateAsync(new TokenProperties());
        return new Certificate(Token.From(token.Value), Token.From(refreshToken.Value));
    }
}