using Commands.Users;
using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Users;

namespace Web.Endpoints.V1.Users.Create;

internal sealed class Endpoint : Endpoint<CreateUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users");
        Permissions("user_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var user = await _mediator.Send(req, ct);

        await SendCreatedAtAsync<Details.Endpoint>(new { user.Id }, new UserResponse
            {
                Id = user.Id,
                Usernames = user.Usernames.Select(username => username).ToList(),
                Status = user.Status.ToString(),
                Groups = user.Groups.Select(id => id).ToList(),
                Roles = user.Roles.Select(id => id).ToList()
            },
            generateAbsoluteUrl: true,
            cancellation: ct);
    }
}

internal sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new user in the system";
        Description = "Create a new user in the system";
        Response(201, "User was successfully created");
    }
}

internal sealed class RequestValidator : Validator<CreateUserCommand>
{
    public RequestValidator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Enter Username")
            .NotNull().WithMessage("Enter Username");

        RuleFor(request => request.GroupId)
            .NotEmpty().WithMessage("Enter Group")
            .NotNull().WithMessage("Enter Group");
    }
}