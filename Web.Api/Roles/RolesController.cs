using Kite.CQRS;
using KunderaNet.AspNetCore.Authorization;
using Managements.Application.Roles;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Types;
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
    [Authorize("roles_create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Authorize("roles_list")]
    public async Task<IActionResult> RolesAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new RolesQuery {Name = name};
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:guid:required}/permissions")]
    [Authorize("roles_permissions_list")]
    public async Task<IActionResult> RolePermissionsAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new RolePermissionsQuery(RoleId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    [Authorize("roles_get")]
    public async Task<IActionResult> RoleAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new RoleQuery(RoleId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:required:guid}")]
    [Authorize("roles_delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand(RoleId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/permission")]
    [Authorize("roles_add_permission")]
    public async Task<IActionResult> AddPermissionAsync([FromRoute] Guid id,
        [FromBody] AddRolePermissionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/permission")]
    [Authorize("roles_remove_permission")]
    public async Task<IActionResult> RemovePermissionAsync([FromRoute] Guid id,
        [FromBody] RemoveRolePermissionRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/meta")]
    [Authorize("roles_add_meta")]
    public async Task<IActionResult> AddMetaAsync([FromRoute] Guid id,
        [FromBody] AddRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/meta")]
    [Authorize("roles_remove_meta")]
    public async Task<IActionResult> RemoveMetaAsync([FromRoute] Guid id,
        [FromBody] RemoveRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}