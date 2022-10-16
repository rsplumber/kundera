using Auth.Application.Authentication;
using Auth.Domain.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Auth;

[ApiController]
public class CredentialController : AbstractAuthController
{
    private readonly ICredentialService _credentialService;

    public CredentialController(ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }

    [HttpPost("users/{id:required:guid}/credentials")]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid id,
        [FromBody] CreateCredentialRequest request,
        CancellationToken cancellationToken)
    {
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        await _credentialService.CreateAsync(uniqueIdentifier, request.Password, id, IpAddress(), cancellationToken);

        return Ok();
    }

    [HttpPost("users/{id:required:guid}/credentials/one-time")]
    public async Task<IActionResult> CreateOneTimeAsync([FromRoute] Guid id,
        [FromBody] CreateOneTimeCredentialRequest request,
        CancellationToken cancellationToken)
    {
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        await _credentialService.CreateOneTimeAsync(uniqueIdentifier, request.Password, id, request.ExpirationTimeInSeconds, IpAddress(), cancellationToken);

        return Ok();
    }

    [HttpPost("users/{id:required:guid}/credentials/time-periodic")]
    public async Task<IActionResult> CreateTimePeriodicAsync([FromRoute] Guid id,
        [FromBody] CreateTimePeriodicCredentialRequest request,
        CancellationToken cancellationToken)
    {
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        await _credentialService.CreateTimePeriodicAsync(uniqueIdentifier, request.Password, id, request.ExpirationTimeInSeconds, IpAddress(), cancellationToken);

        return Ok();
    }

    [HttpPatch("credentials/{uniqueIdentifier:required}/change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromRoute] string uniqueIdentifier,
        [FromBody] CredentialChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        await _credentialService.ChangePasswordAsync(UniqueIdentifier.Parse(uniqueIdentifier), request.Password, request.NewPassword, IpAddress(), cancellationToken);

        return Ok();
    }

    [HttpDelete("credentials/{uniqueIdentifier:required}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string uniqueIdentifier, CancellationToken cancellationToken)
    {
        await _credentialService.RemoveAsync(UniqueIdentifier.Parse(uniqueIdentifier), cancellationToken);

        return Ok();
    }
}