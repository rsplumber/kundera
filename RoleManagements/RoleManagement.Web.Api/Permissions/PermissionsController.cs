using Microsoft.AspNetCore.Mvc;
using RoleManagement.Application.Permissions;
using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS;

namespace RoleManagement.Web.Api.Permissions;

[ApiController]
[Route("/permissions")]
public class PermissionsController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public PermissionsController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpGet]
    public async Task<IActionResult> PermissionsAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new PermissionsQuery
        {
            Name = name
        };
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