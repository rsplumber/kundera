using Kite.CQRS;
using Managements.Application.Roles;
using Managements.Domain.Roles;
using Microsoft.AspNetCore.Mvc;

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
        var query = new RolesQuery {Name = name};
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    public async Task<IActionResult> RoleAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new RoleQuery(RoleId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:required:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(RoleId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/permission")]
    public async Task<IActionResult> AddPermissionAsync([FromRoute] Guid id,
        [FromBody] AddRolePermissionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/permission")]
    public async Task<IActionResult> RemovePermissionAsync([FromRoute] Guid id,
        [FromBody] RemoveRolePermissionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/meta")]
    public async Task<IActionResult> AddMetaAsync([FromRoute] Guid id,
        [FromBody] AddRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/meta")]
    public async Task<IActionResult> RemoveMetaAsync([FromRoute] Guid id,
        [FromBody] RemoveRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}