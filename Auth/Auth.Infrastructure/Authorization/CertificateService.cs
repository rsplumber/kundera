﻿using Auth.Application.Authorization;
using Kite.Tokens;
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
        var token = await _tokenService.GenerateAsync(TokenProperties.Create()
                                                                     .Add("id", id)
                                                                     .Add("scope", scope));

        var refreshToken = await _tokenService.GenerateAsync(TokenProperties.Create());

        return new Certificate(Token.From(token.Value), Token.From(refreshToken.Value));
    }
}