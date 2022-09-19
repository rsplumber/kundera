using Microsoft.AspNetCore.Mvc;
using Tes.CQRS;
using Users.Application.UserGroups;
using Users.Application.Users;
using Users.Domain.UserGroups;
using Users.Domain.Users;
using Users.Web.Api.Users;

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

    //Todo joda kardane Roles az Create va sakhte 2 Api mozaja baraye ezafe va kam kardan roles
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateUserGroupRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
    

    //Todo name koja estefade shode?!
    [HttpGet]
    public async Task<IActionResult> UserGroupsAsync([FromQuery] string? name, CancellationToken cancellationToken)
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