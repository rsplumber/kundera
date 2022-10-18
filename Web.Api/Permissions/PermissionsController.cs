using Kite.CQRS;
using Managements.Application.Permissions;
using Managements.Domain.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Permissions;

[ApiController]
[Route("/permissions")]
public class PermissionsController : ControllerBase
{
    //Todo Baraye Add Permission che qalati konim namoosan?

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

    [HttpGet("{id:required}")]
    public async Task<IActionResult> PermissionAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new PermissionQuery(PermissionId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:required}")]
    public async Task<IActionResult> RemoveMetaAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new DeletePermissionCommand(PermissionId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpPost("{id:required}/meta")]
    public async Task<IActionResult> AddMetaAsync([FromRoute] string id, [FromBody] AddPermissionMetaRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required}/meta")]
    public async Task<IActionResult> RemoveMetaAsync([FromRoute] string id, [FromBody] RemovePermissionMetaRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}