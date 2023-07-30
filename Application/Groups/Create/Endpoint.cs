using System.Net;
using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Groups;

namespace Application.Groups.Create;

file sealed class Endpoint : Endpoint<CreateGroupCommand>
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

        await SendAsync(new GroupResponse
        {
            Id = group.Id,
            Description = group.Description,
            Parent = group.Parent?.Id,
            Name = group.Name,
            Status = group.Status.ToString(),
            StatusChangedDate = group.StatusChangeDateUtc,
        }, (int)HttpStatusCode.Created, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Creates a new group in the system";
        Description = "Creates a new group in the system";
        Response(201, "Group was successfully created");
    }
}

file sealed class RequestValidator : Validator<CreateGroupCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage("Enter Name")
            .NotNull().WithMessage("Enter Name");

        RuleFor(request => request.RoleId)
            .NotEmpty().WithMessage("Enter Name")
            .NotNull().WithMessage("Enter Name");
    }
}