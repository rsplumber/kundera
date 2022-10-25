using Kite.CQRS;
using KunderaNet.AspNetCore.Authorization;
using Managements.Application.Groups;
using Managements.Domain.Groups;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Groups;

[ApiController]
[Route("/groups")]
public class GroupsController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public GroupsController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpPost]
    [Authorize("groups_create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Authorize("groups_list")]
    public async Task<IActionResult> GroupsAsync(CancellationToken cancellationToken)
    {
        var query = new GroupsQuery();
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    [Authorize("groups_get")]
    public async Task<IActionResult> GroupAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GroupQuery(GroupId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:required:guid}")]
    [Authorize("groups_delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteGroupCommand(GroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/roles")]
    [Authorize("groups_assign_role")]
    public async Task<IActionResult> AssignRole([FromRoute] Guid id, [FromBody] AssignGroupRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/roles")]
    [Authorize("groups_revoke_role")]
    public async Task<IActionResult> RevokeRole([FromRoute] Guid id, [FromBody] RevokeGroupRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/parent")]
    [Authorize("groups_set_parent")]
    public async Task<IActionResult> SetParent([FromRoute] Guid id, [FromBody] SetGroupParentRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/parent/move")]
    [Authorize("groups_move_parent")]
    public async Task<IActionResult> MoveParent([FromRoute] Guid id, [FromBody] MoveGroupParentRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/parent")]
    [Authorize("groups_remove_parent")]
    public async Task<IActionResult> RemoveParent([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveGroupParentCommand(GroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpPost("{id:required:guid}/enable")]
    [Authorize("groups_enable")]
    public async Task<IActionResult> EnableAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new EnableGroupCommand(GroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/disable")]
    [Authorize("groups_disable")]
    public async Task<IActionResult> DisableAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DisableGroupCommand(GroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}