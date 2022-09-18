using Microsoft.AspNetCore.Mvc;
using RoleManagement.Application.Services;
using RoleManagements.Domain.Services.Types;
using Tes.CQRS;

namespace RoleManagement.Web.Api.Services;

[ApiController]
[Route("/services")]
public class ServicesController : ControllerBase
{
    private readonly IServiceBus _serviceBus;

    public ServicesController(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateServiceRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ServicesAsync([FromQuery] string? name, CancellationToken cancellationToken)
    {
        var query = new ServicesQuery
        {
            Name = name
        };
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required}")]
    public async Task<IActionResult> ServiceAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var query = new ServiceQuery(ServiceId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);
        return Ok(response);
    }

    [HttpPatch("{id:required}/activate")]
    public async Task<IActionResult> ActivateAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new ActivateServiceCommand(ServiceId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }

    [HttpPatch("{id:required}/de-activate")]
    public async Task<IActionResult> DeActivateAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        var command = new DeActivateServiceCommand(ServiceId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);
        return Ok();
    }
}