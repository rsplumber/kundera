using System.Net;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Api;

[ApiController]
public class CredentialController : ControllerBase
{
    private readonly ICredentialService _credentialService;
    private readonly IOneTimeCredentialService _oneTimeCredentialService;
    private readonly ITimePeriodicCredentialService _timePeriodicCredentialService;

    public CredentialController(ICredentialService credentialService,
        IOneTimeCredentialService oneTimeCredentialService,
        ITimePeriodicCredentialService timePeriodicCredentialService)
    {
        _credentialService = credentialService;
        _oneTimeCredentialService = oneTimeCredentialService;
        _timePeriodicCredentialService = timePeriodicCredentialService;
    }

    [HttpPost("users/{id:required:guid}/credentials")]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid id,
        [FromBody] CreateCredentialRequest request, CancellationToken cancellationToken)
    {
        var userId = UserId.From(id);
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        var password = Password.From(request.Password);
        await _credentialService.CreateAsync(uniqueIdentifier, userId, password, IpAddress(), cancellationToken);
        return Ok();
    }

    [HttpPost("users/{id:required:guid}/credentials/one-time")]
    public async Task<IActionResult> CreateOneTimeAsync(
        [FromRoute] Guid id,
        [FromBody] CreateOneTimeCredentialRequest request, CancellationToken cancellationToken)
    {
        var userId = UserId.From(id);
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        var password = Password.From(request.Password);
        await _oneTimeCredentialService.CreateAsync(uniqueIdentifier, userId, password, request.ExpirationTimeInSeconds, IpAddress(), cancellationToken);
        return Ok();
    }

    [HttpPost("users/{id:required:guid}/credentials/time-periodic")]
    public async Task<IActionResult> CreateTimePeriodicAsync(
        [FromRoute] Guid id,
        [FromBody] CreateTimePeriodicCredentialRequest request, CancellationToken cancellationToken)
    {
        var userId = UserId.From(id);
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        var password = Password.From(request.Password);
        await _timePeriodicCredentialService.CreateAsync(uniqueIdentifier, userId, password, request.ExpirationTimeInSeconds, IpAddress(), cancellationToken);
        return Ok();
    }

    [HttpPatch("credentials/{uniqueIdentifier:required}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] string uniqueIdentifier,
        [FromBody] UpdateCredentialRequest request, CancellationToken cancellationToken)
    {
        var password = Password.From(request.Password);
        await _credentialService.UpdateAsync(UniqueIdentifier.Parse(uniqueIdentifier), password, IpAddress(), cancellationToken);
        return Ok();
    }

    [HttpDelete("credentials/{uniqueIdentifier:required}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string uniqueIdentifier, CancellationToken cancellationToken)
    {
        await _credentialService.RemoveAsync(UniqueIdentifier.Parse(uniqueIdentifier), cancellationToken);
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