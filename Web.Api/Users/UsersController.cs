using Kite.CQRS;
using KunderaNet.AspNetCore.Authorization;
using Managements.Application.Users;
using Managements.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Users;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public UsersController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpPost]
    [Authorize("user_create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpGet]
    [Authorize("user_list")]
    public async Task<IActionResult> UsersAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new UsersQuery();
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    [Authorize("user_get")]
    public async Task<IActionResult> UserAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new UserQuery(UserId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpDelete("{id:required:guid}")]
    [Authorize("user_delete")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(UserId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpPost("{id:required:guid}/usernames")]
    [Authorize("user_add_username")]
    public async Task<IActionResult> AddUsernameAsync([FromRoute] Guid id, [FromBody] AddUserUsernameRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/usernames")]
    [Authorize("user_remove_username")]
    public async Task<IActionResult> RemoveUsernameAsync([FromRoute] Guid id, [FromBody] RemoveUserUsernameRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("/usernames/{username:required}/check")]
    [Authorize("user_exist_username")]
    public async Task<IActionResult> ExistUsernameAsync([FromRoute] string username, CancellationToken cancellationToken)
    {
        var query = new ExistUserUsernameQuery(username);
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(new {Exists = response});
    }

    [HttpPost("{id:required:guid}/roles")]
    [Authorize("user_assign_role")]
    public async Task<IActionResult> AssignRoleAsync([FromRoute] Guid id, [FromBody] AssignUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/roles")]
    [Authorize("user_revoke_role")]
    public async Task<IActionResult> RevokeRoleAsync([FromRoute] Guid id, [FromBody] RevokeUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required:guid}/groups")]
    [Authorize("user_join_group")]
    public async Task<IActionResult> JoinGroupAsync([FromRoute] Guid id, [FromBody] JoinUserToGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id:required:guid}/groups")]
    [Authorize("user_remove_group")]
    public async Task<IActionResult> RemoveFromGroupAsync([FromRoute] Guid id, [FromBody] RemoveUserFromGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required}/activate")]
    [Authorize("user_activate")]
    public async Task<IActionResult> ActivateUserAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new ActiveUserCommand(UserId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required}/suspend")]
    [Authorize("user_suspend")]
    public async Task<IActionResult> SuspendFromGroupAsync([FromRoute] Guid id, [FromBody] SuspendUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPost("{id:required}/block")]
    [Authorize("user_block")]
    public async Task<IActionResult> BlockFromGroup([FromRoute] Guid id, [FromBody] BlockUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}