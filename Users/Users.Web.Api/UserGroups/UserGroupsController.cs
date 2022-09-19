using Microsoft.AspNetCore.Mvc;
using Tes.CQRS;
using Users.Application.UserGroups;
using Users.Domain.UserGroups;

namespace Users.Web.Api.UserGroups;

[ApiController]
[Route("/user-groups")]
public class UserGroupsController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public UserGroupsController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost("{id:required:guid}/roles")]
    public async Task<IActionResult> AssignRole([FromRoute] Guid id,[FromBody] AssignUserGroupRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
    
    [HttpDelete("{id:required:guid}/roles")]
    public async Task<IActionResult> RevokeRole([FromRoute] Guid id, [FromBody] RevokeUserGroupRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> UserGroupsAsync(CancellationToken cancellationToken)
    {
        var query = new UserGroupsQuery();
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    public async Task<IActionResult> UserGroupAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new UserGroupQuery(UserGroupId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return Ok(response);
    }
}