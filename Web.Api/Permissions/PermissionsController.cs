using Kite.CQRS;
using Managements.Application.Permissions;
using Managements.Domain.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Permissions;

[ApiController]
[Route("/permissions")]
public class PermissionsController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public PermissionsController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> PermissionsAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new PermissionsQuery {Name = name};
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    public async Task<IActionResult> PermissionAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new PermissionQuery(PermissionId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:required:guid}")]
    public async Task<IActionResult> RemoveMetaAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeletePermissionCommand(PermissionId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpPost("{id:required:guid}/meta")]
    public async Task<IActionResult> AddMetaAsync([FromRoute] Guid id, [FromBody] AddPermissionMetaRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required}/meta")]
    public async Task<IActionResult> RemoveMetaAsync([FromRoute] Guid id, [FromBody] RemovePermissionMetaRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}