using Auth.Core;
using Auth.Core.Entities;
using Auth.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Auth;

[ApiController]
public class SessionsController : AbstractAuthController
{
    private readonly ISessionManagement _sessionManagement;

    public SessionsController(ISessionManagement sessionManagement)
    {
        _sessionManagement = sessionManagement;
    }

    [HttpDelete("sessions/terminate")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] TerminateSessionRequest request, CancellationToken cancellationToken)
    {
        await _sessionManagement.DeleteAsync(Token.From(request.Token), cancellationToken);
        return Ok();
    }

    [HttpGet("users/{id:guid:required}/sessions")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var sessions = await _sessionManagement.GetAllAsync(id, cancellationToken);
        var response = sessions.Select(session => new
        {
            session.Id, Scope = session.ScopeId, session.ExpiresAt, session.UserId, session.LastIpAddress, session.LastUsageDate
        });
        return Ok(response);
    }
}