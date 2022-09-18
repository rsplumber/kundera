using Microsoft.AspNetCore.Mvc;
using RoleManagement.Application.Scopes;
using RoleManagements.Domain.Scopes.Types;
using Tes.CQRS;
using Controller = Tes.Web.Controllers.Controller;

namespace RoleManagement.Web.Api.Scopes;

[Route("/scopes")]
public class ScopesController : Controller
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
        return CreateResponse();
    }

    [HttpGet]
    public async Task<IActionResult> ScopesAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new ScopesQuery
        {
            Name = name
        };
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return CreateResponse(response);
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> ScopeAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new ScopeQuery(ScopeId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return CreateResponse(response);
    }

    [HttpPost("{id:required}/services")]
    public async Task<IActionResult> AddServiceAsync(
        [FromRoute] string id,
        [FromBody] AddScopeServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }

    [HttpDelete("{id:required}/services")]
    public async Task<IActionResult> RemoveServiceAsync(
        [FromRoute] string id,
        [FromBody] RemoveScopeServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }


    [HttpPost("{id:required}/roles")]
    public async Task<IActionResult> AddRoleAsync(
        [FromRoute] string id,
        [FromBody] AddScopeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }

    [HttpDelete("{id:required}/roles")]
    public async Task<IActionResult> RemoveRoleAsync(
        [FromRoute] string id,
        [FromBody] RemoveScopeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return CreateResponse();
    }
}