using System.Net;
using Auth.Application.Authorization;
using Auth.Domain.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Auth;

[ApiController]
[Route("/authorize")]
public class AuthorizeController : ControllerBase
{
    private readonly IAuthorizeService _authorizeService;

    public AuthorizeController(IAuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }

    [HttpPost]
    public async Task<IActionResult> AuthorizeAsync([FromBody] AuthorizeRequest request,
        [FromHeader] string authorization,
        [FromHeader] string? scope = "global",
        [FromHeader] string? service = "all",
        CancellationToken cancellationToken = default)
    {
        var response = await _authorizeService.AuthorizeAsync(Token.From(authorization), request.Action, scope, service, IpAddress(), cancellationToken);
        return Ok(response);
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