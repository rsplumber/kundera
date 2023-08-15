using Data.Abstractions.Permissions;
using FastEndpoints;
using FluentValidation;
using Mediator;

namespace Application.Services.Permissions.Create;

file sealed class Endpoint : Endpoint<CreatePermissionCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("services/{serviceId}/permissions");
        Permissions("permissions_create");
        Version(1);
    }

    public override async Task HandleAsync(CreatePermissionCommand req, CancellationToken ct)
    {
        var permission = await _mediator.Send(req, ct);
        await SendCreatedAtAsync<Application.Permissions.Details.Endpoint>(new { permission.Id }, new PermissionResponse
            {
                Id = permission.Id,
                Name = permission.Name,
                Meta = permission.Meta
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new Permission in the system";
        Description = "Create a new Permission in the system";
        Response(201, "Permission was successfully created");
    }
}

file sealed class RequestValidator : Validator<CreatePermissionCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.ServiceId)
            .NotEmpty().WithMessage("Enter a ServiceId")
            .NotNull().WithMessage("Enter a ServiceId");

        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter a Name")
            .NotNull().WithMessage("Enter a Name");
    }
}