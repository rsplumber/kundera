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
    public async Task<IActionResult> CreateByUsernameAsync([FromBody] CreateUserByUsernameRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateByPhoneNumberAsync([FromBody] CreateUserByPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateByEmailAsync([FromBody] CreateUserByEmailRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateByNationalCodeAsync([FromBody] CreateUserByNationalCodeRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> AssignRole([FromBody] AssignUserRoleRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> RevokeRole([FromBody] RevokeUserRoleRequest request, CancellationToken cancellationToken)
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
    
    //Todo Api baraye join shodan be group! nmidonam bayad toye kodom controller bashe, ono khodet peyda kon vali nadidamesh
    
    

}