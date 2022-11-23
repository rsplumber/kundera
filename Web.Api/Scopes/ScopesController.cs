using Kite.CQRS;
using KunderaNet.AspNetCore.Authorization;
using Managements.Application.Scopes;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Types;
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
    [Authorize("scopes_create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateScopeRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Authorize("scopes_list")]
    public async Task<IActionResult> ScopesAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new ScopesQuery {Name = name};
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid:required}/roles")]
    [Authorize("scopes_roles_list")]
    public async Task<IActionResult> ScopeRolesAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new ScopeRolesQuery(ScopeId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    [Authorize("scopes_get")]
    public async Task<IActionResult> ScopeAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new ScopeQuery(ScopeId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }


    [HttpDelete("{id:required:guid}")]
    [Authorize("scopes_delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteScopeCommand(ScopeId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/active")]
    [Authorize("scopes_active")]
    public async Task<IActionResult> ActiveAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateScopeCommand(ScopeId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required:guid}/de-active")]
    [Authorize("scopes_de-active")]
    public async Task<IActionResult> DeActiveAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeActivateScopeCommand(ScopeId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required:guid}/services")]
    [Authorize("scopes_add_service")]
    public async Task<IActionResult> AddServiceAsync([FromRoute] Guid id,
        [FromBody] AddScopeServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/services")]
    [Authorize("scopes_remove_service")]
    public async Task<IActionResult> RemoveServiceAsync([FromRoute] Guid id,
        [FromBody] RemoveScopeServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpPost("{id:required:guid}/roles")]
    [Authorize("scopes_add_role")]
    public async Task<IActionResult> AddRoleAsync([FromRoute] Guid id,
        [FromBody] AddScopeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/roles")]
    [Authorize("scopes_remove_role")]
    public async Task<IActionResult> RemoveRoleAsync([FromRoute] Guid id,
        [FromBody] RemoveScopeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}