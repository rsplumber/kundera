using Microsoft.AspNetCore.Mvc;
using RoleManagement.Application.Roles;
using RoleManagements.Domain.Roles.Types;
using Tes.CQRS;
using Controller = Tes.Web.Controllers.Controller;

namespace RoleManagement.Web.Api.Roles;

[Route("/roles")]
public class RolesController : Controller
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
        return CreateResponse();
    }

    [HttpGet]
    public async Task<IActionResult> RolesAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new RolesQuery
        {
            Name = name
        };
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return CreateResponse(response);
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> RoleAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new RoleQuery(RoleId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return CreateResponse(response);
    }

    [HttpPost("{id:required}/meta")]
    public async Task<IActionResult> AddMetaAsync(
        [FromRoute] string id,
        [FromBody] AddRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }

    [HttpDelete("{id:required}/meta")]
    public async Task<IActionResult> RemoveMetaAsync(
        [FromRoute] string id,
        [FromBody] RemoveRoleMetaRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }
}