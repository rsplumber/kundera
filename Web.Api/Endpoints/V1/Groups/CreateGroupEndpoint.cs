using Application.Groups;
using FastEndpoints;
using Mediator;

namespace Web.Api.Endpoints.V1.Groups;

internal sealed class CreateGroupEndpoint : Endpoint<CreateGroupCommand>
{
    private readonly IMediator _mediator;

    public CreateGroupEndpoint(IMediator mediator)
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

        await SendCreatedAtAsync<GroupEndpoint>(new {group.Id}, new GroupResponse
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

internal sealed class CreateGroupEndpointSummary : Summary<CreateGroupEndpoint>
{
    public CreateGroupEndpointSummary()
    {
        Summary = "Creates a new group in the system";
        Description = "Creates a new group in the system";
        Response(201, "Group was successfully created");
    }
}