using Microsoft.AspNetCore.Mvc;
using Tes.CQRS;
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

    [HttpPost]
    public async Task<IActionResult> AssignRoleAsync([FromBody] AssignUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRoleAsync([FromBody] RevokeUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> JoinGroupAsync([FromBody] JoinUserToGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromGroupAsync([FromBody] RemoveUserFromGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> ActivateUserAsync([FromBody] ActiveUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> SuspendFromGroupAsync([FromBody] SuspendUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> BlockFromGroup([FromBody] BlockUserStatusRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
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