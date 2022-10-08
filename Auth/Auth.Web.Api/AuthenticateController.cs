using System.Net;
using Auth.Application;
using Auth.Domain.Credentials;
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
        var password = Password.From(request.Password);
        var certificate = await _authenticateService.AuthenticateAsync(uniqueIdentifier,
            password,
            request.Scope,
            IpAddress(),
            cancellationToken);
        return Ok(certificate);
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