using System.Net;
using Auth.Application.Authentication;
using Auth.Domain.Credentials;
using Auth.Domain.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Api;

[ApiController]
[Route("/authenticate")]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthenticateService _authenticateService;

    public AuthenticateController(IAuthenticateService authenticateService)
    {
        _authenticateService = authenticateService;
    }

    [HttpPost]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequest request, CancellationToken cancellationToken)
    {
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        var (token, refreshToken) = await _authenticateService.AuthenticateAsync(uniqueIdentifier,
            request.Password,
            request.Scope,
            IpAddress(),
            cancellationToken);
        return Ok(new
        {
            Token = token.Value,
            RefreshToken = refreshToken.Value
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromHeader] string token, [FromBody] RefreshRequest request, CancellationToken cancellationToken)
    {
        var (newToken, refreshToken) = await _authenticateService.RefreshCertificateAsync(Token.From(token), Token.From(request.RefreshToken), IpAddress(), cancellationToken);
        return Ok(new
        {
            Token = newToken.Value,
            RefreshToken = refreshToken.Value
        });
    }

    private IPAddress IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Real-IP"))
        {
            return IPAddress.Parse(Request.Headers["X-Real-IP"]);
        }

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4() ?? IPAddress.None;
    }
}