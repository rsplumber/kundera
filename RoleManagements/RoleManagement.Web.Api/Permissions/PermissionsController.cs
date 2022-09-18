using Microsoft.AspNetCore.Mvc;
using RoleManagement.Application.Permissions;
using RoleManagements.Domain.Permissions.Types;
using Tes.CQRS;
using Controller = Tes.Web.Controllers.Controller;

namespace RoleManagement.Web.Api.Permissions;

[Route("/permissions")]
public class PermissionsController : Controller
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

        return CreateResponse(response);
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> PermissionAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new PermissionQuery(PermissionId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return CreateResponse(response);
    }

    [HttpPost("{id:required}/meta")]
    public async Task<IActionResult> AddMetaAsync(
        [FromRoute] string id,
        [FromBody] AddPermissionMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }

    [HttpDelete("{id:required}/meta")]
    public async Task<IActionResult> RemoveMetaAsync(
        [FromRoute] string id,
        [FromBody] RemovePermissionMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }
}