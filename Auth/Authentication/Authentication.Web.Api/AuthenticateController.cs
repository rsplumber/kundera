using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;
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

    //Todo Add IpAddress
    [HttpPost]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequest request, CancellationToken cancellationToken)
    {
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        var password = Password.From(request.Password);
        var oneTimeToken = await _authenticateService.AuthenticateAsync(uniqueIdentifier, password, cancellationToken: cancellationToken);
        return Ok(oneTimeToken);
    }
}