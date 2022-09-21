using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Api;

[ApiController]
public class CredentialController : ControllerBase
{
    private readonly ICredentialService _credentialService;


    public CredentialController(ICredentialService credentialService)
    {
        _credentialService = credentialService;
    }

    [HttpPost("users/{id:required:guid}/credentials")]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid id,
        [FromBody] CreateCredentialRequest request, CancellationToken cancellationToken)
    {
        var userId = UserId.From(id);
        var uniqueIdentifier = UniqueIdentifier.From(request.Username, request.Type);
        var password = Password.From(request.Password);
        await _credentialService.CreateAsync(uniqueIdentifier, userId, password, cancellationToken);
        return Ok();
    }

    [HttpPatch("credentials/{uniqueIdentifier:required}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] string uniqueIdentifier,
        [FromBody] UpdateCredentialRequest request, CancellationToken cancellationToken)
    {
        var password = Password.From(request.Password);
        await _credentialService.UpdateAsync(UniqueIdentifier.Parse(uniqueIdentifier), password, cancellationToken);
        return Ok();
    }

    [HttpDelete("credentials/{uniqueIdentifier:required}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string uniqueIdentifier, CancellationToken cancellationToken)
    {
        await _credentialService.RemoveAsync(UniqueIdentifier.Parse(uniqueIdentifier), cancellationToken);
        return Ok();
    }
}