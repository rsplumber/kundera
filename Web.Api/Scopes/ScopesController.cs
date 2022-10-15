using Application.Scopes;
using Domain.Scopes;
using Kite.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Scopes;

[ApiController]
[Route("/scopes")]
public class ScopesController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public ScopesController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateScopeRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ScopesAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new ScopesQuery {Name = name};
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> ScopeAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new ScopeQuery(ScopeId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }


    [HttpDelete("{id:required}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new DeleteScopeCommand(ScopeId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required}/active")]
    public async Task<IActionResult> ActiveAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new ActivateScopeCommand(ScopeId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required}/de-active")]
    public async Task<IActionResult> DeActiveAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new DeActivateScopeCommand(ScopeId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required}/services")]
    public async Task<IActionResult> AddServiceAsync([FromRoute] string id,
        [FromBody] AddScopeServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required}/services")]
    public async Task<IActionResult> RemoveServiceAsync([FromRoute] string id,
        [FromBody] RemoveScopeServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpPost("{id:required}/roles")]
    public async Task<IActionResult> AddRoleAsync([FromRoute] string id,
        [FromBody] AddScopeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required}/roles")]
    public async Task<IActionResult> RemoveRoleAsync([FromRoute] string id,
        [FromBody] RemoveScopeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}