using Auth.Core;
using Auth.Core.Entities;
using Auth.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Auth;

[ApiController]
[Route("/authenticate")]
public class AuthenticateController : AbstractAuthController
{
    private readonly IAuthenticateService _authenticateService;

    public AuthenticateController(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    [HttpPost]
    public async Task<IActionResult> AuthenticateAsync([FromHeader] string scopeSecret,
        [FromBody] AuthenticateRequest request,
        CancellationToken cancellationToken)
    {
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        var (token, refreshToken) = await _authenticateService.AuthenticateAsync(uniqueIdentifier,
            request.Password,
            scopeSecret,
            IpAddress(),
            cancellationToken);
        return Ok(new
        {
            Token = token.Value,
            RefreshToken = refreshToken.Value
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromHeader] string authorization, [FromBody] RefreshRequest request, CancellationToken cancellationToken)
    {
        var (newToken, refreshToken) = await _authenticateService.RefreshCertificateAsync(Token.From(authorization), Token.From(request.RefreshToken), IpAddress(), cancellationToken);
        return Ok(new
        {
            Token = newToken.Value,
            RefreshToken = refreshToken.Value
        });
    }
}