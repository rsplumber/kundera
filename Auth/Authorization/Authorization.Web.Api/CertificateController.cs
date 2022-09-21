using Authorization.Application;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Web.Api;

[ApiController]
[Route("authorization/certificates")]
public class CertificateController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }

    [HttpPost]
    public async Task<IActionResult> AuthenticateAsync([FromBody] CertificateRequest request, CancellationToken cancellationToken)
    {
        var certificate = await _certificateService.GenerateAsync(request.OneTimeToken, cancellationToken);
        return Ok(certificate);
    }
}