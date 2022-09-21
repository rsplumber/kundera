using Microsoft.AspNetCore.Mvc;
using Tes.CQRS;
using Tes.CQRS.Contracts;
using Users.Application.Users;
using Users.Domain.Users;

namespace Users.Web.Api.Users;

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
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required:guid}/usernames")]
    public async Task<IActionResult> AddUsernameAsync([FromRoute] Guid id, [FromBody] AddUserUsernameRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:required:guid}/usernames")]
    public async Task<IActionResult> RemoveUsernameAsync([FromRoute] Guid id, [FromBody] RemoveUserUsernameRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required:guid}/usernames/{username:required}/check")]
    public async Task<IActionResult> ExistUsernameAsync([FromRoute] Guid id, [FromRoute] string username, CancellationToken cancellationToken)
    {
        var command = new ExistUserUsernameCommand(UserId.From(id), username);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required:guid}/roles")]
    public async Task<IActionResult> AssignRoleAsync([FromRoute] Guid id, [FromBody] AssignUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:required:guid}/roles")]
    public async Task<IActionResult> RevokeRoleAsync([FromRoute] Guid id, [FromBody] RevokeUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required:guid}/groups")]
    public async Task<IActionResult> JoinGroupAsync([FromRoute] Guid id, [FromBody] JoinUserToGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:required:guid}/groups")]
    public async Task<IActionResult> RemoveFromGroupAsync([FromRoute] Guid id, [FromBody] RemoveUserFromGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required}/activate")]
    public async Task<IActionResult> ActivateUserAsync([FromRoute] Guid id, [FromBody] ActiveUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required}/suspend")]
    public async Task<IActionResult> SuspendFromGroupAsync([FromRoute] Guid id, [FromBody] SuspendUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:required}/block")]
    public async Task<IActionResult> BlockFromGroup([FromRoute] Guid id, [FromBody] BlockUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> UsersAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new UsersQuery();
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    public async Task<IActionResult> UserAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new UserQuery(UserId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return Ok(response);
    }
}