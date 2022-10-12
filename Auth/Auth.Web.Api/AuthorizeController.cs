using System.Net;
using Auth.Application.Authorization;
using Auth.Domain.Sessions;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Api;

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
        [FromHeader] string token,
        [FromHeader] string scope = "global",
        [FromHeader] string? service = "all",
        CancellationToken cancellationToken = default)
    {
        await _authorizeService.AuthorizeAsync(Token.From(token), request.Action, scope, service, IpAddress(), cancellationToken);
        return Ok();
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