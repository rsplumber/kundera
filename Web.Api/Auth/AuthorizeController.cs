using Auth.Core;
using Auth.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Auth;

[ApiController]
[Route("/authorize")]
public class AuthorizeController : AbstractAuthController
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
}