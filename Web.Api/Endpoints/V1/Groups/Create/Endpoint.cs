using Application.Groups;
using FastEndpoints;
using Mediator;
using Web.Api.Endpoints.V1.Groups.Details;

namespace Web.Api.Endpoints.V1.Groups.Create;

internal sealed class Endpoint : Endpoint<CreateGroupCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("groups");
        Permissions("groups_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateGroupCommand req, CancellationToken ct)
    {
        var group = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<Details.Endpoint>(new {group.Id}, new GroupResponse
            {
                Id = group.Id.Value,
                Description = group.Description,
                Parent = group.Parent!.Value,
                Name = group.Name,
                Status = group.Status.Name,
                StatusChangedDate = group.StatusChangeDate,
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Creates a new group in the system";
        Description = "Creates a new group in the system";
        Response(201, "Group was successfully created");
    }
}