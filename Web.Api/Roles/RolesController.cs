using Application.Roles;
using Domain.Roles;
using Microsoft.AspNetCore.Mvc;
using Tes.CQRS;

namespace Web.Api.Roles;

[ApiController]
[Route("/roles")]
public class RolesController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public RolesController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> RolesAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new RolesQuery
        {
            Name = name
        };
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> RoleAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new RoleQuery(RoleId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{id:required}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] string id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(RoleId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:required}/permission")]
    public async Task<IActionResult> AddPermissionAsync(
        [FromRoute] string id,
        [FromBody] AddRolePermissionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required}/meta")]
    public async Task<IActionResult> AddMetaAsync(
        [FromRoute] string id,
        [FromBody] AddRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:required}/meta")]
    public async Task<IActionResult> RemoveMetaAsync(
        [FromRoute] string id,
        [FromBody] RemoveRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
}