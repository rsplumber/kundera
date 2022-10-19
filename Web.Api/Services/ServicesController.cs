using Kite.CQRS;
using Managements.Application.Services;
using Managements.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Services;

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
        var query = new ServicesQuery {Name = name};
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }

    [HttpGet("{id:required:guid}")]
    public async Task<IActionResult> ServiceAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new ServiceQuery(ServiceId.From(id));
        var response = await _serviceBus.QueryAsync(query, cancellationToken);

        return Ok(response);
    }


    [HttpDelete("{id:required:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteServiceCommand(ServiceId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }


    [HttpPatch("{id:required:guid}/activate")]
    public async Task<IActionResult> ActivateAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateServiceCommand(ServiceId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }

    [HttpPatch("{id:required:guid}/de-activate")]
    public async Task<IActionResult> DeActivateAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeActivateServiceCommand(ServiceId.From(id));
        await _serviceBus.SendAsync(command, cancellationToken);

        return Ok();
    }
}