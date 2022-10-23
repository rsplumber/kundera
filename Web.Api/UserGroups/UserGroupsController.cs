using Kite.CQRS;
using KunderaNet.AspNetCore.Authorization;
using Managements.Application.UserGroups;
using Managements.Domain.UserGroups;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.UserGroups;

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
    [Authorize("user-groups_create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
    
    [HttpGet]
    [Authorize("user-groups_list")]
    public async Task<IActionResult> UserGroupsAsync(CancellationToken cancellationToken)
    {
        var query = new UserGroupsQuery();
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    [Authorize("user-groups_get")]
    public async Task<IActionResult> UserGroupAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new UserGroupQuery(UserGroupId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }
    
    [HttpDelete("{id:required:guid}")]
    [Authorize("user-groups_delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserGroupCommand(UserGroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/roles")]
    [Authorize("user-groups_assign_role")]
    public async Task<IActionResult> AssignRole([FromRoute] Guid id, [FromBody] AssignUserGroupRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/roles")]
    [Authorize("user-groups_revoke_role")]
    public async Task<IActionResult> RevokeRole([FromRoute] Guid id, [FromBody] RevokeUserGroupRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/parent")]
    [Authorize("user-groups_set_parent")]
    public async Task<IActionResult> SetParent([FromRoute] Guid id, [FromBody] SetUserGroupParentRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/parent/move")]
    [Authorize("user-groups_move_parent")]
    public async Task<IActionResult> MoveParent([FromRoute] Guid id, [FromBody] MoveUserGroupParentRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/parent")]
    [Authorize("user-groups_remove_parent")]
    public async Task<IActionResult> RemoveParent([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new RemoveUserGroupParentCommand(UserGroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
    

    [HttpPost("{id:required:guid}/enable")]
    [Authorize("user-groups_enable")]
    public async Task<IActionResult> EnableAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new EnableUserGroupCommand(UserGroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/disable")]
    [Authorize("user-groups_disable")]
    public async Task<IActionResult> DisableAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DisableUserGroupCommand(UserGroupId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}