using Auth.Core;
using Auth.Core.Entities;
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
    public async Task<IActionResult> AuthorizeAsync([FromHeader] string authorization,
        [FromHeader] string action,
        [FromHeader] string serviceSecret,
        CancellationToken cancellationToken = default)
    {
        var response = await _authorizeService.AuthorizeAsync(Token.From(authorization), action, serviceSecret, IpAddress(), cancellationToken);
        return Ok(response);
    }
}