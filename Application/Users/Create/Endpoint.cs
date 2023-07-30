using System.Net;
using FastEndpoints;
using FluentValidation;
using Mediator;
using Queries.Users;

namespace Application.Users.Create;

file sealed class Endpoint : Endpoint<CreateUserCommand>
{
    private readonly IMediator _mediator;

    public Endpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("users");
        Permissions("users_create");
        Version(1);
    }

    public override async Task HandleAsync(CreateUserCommand req, CancellationToken ct)
    {
        var user = await _mediator.Send(req, ct);
        await SendAsync(new UserResponse
        {
            Id = user.Id,
            Usernames = user.Usernames.Select(username => username).ToList(),
            Status = user.Status.ToString(),
            Groups = user.Groups.Select(g => g.Id).ToList(),
            Roles = user.Roles.Select(r => r.Id).ToList()
        }, (int)HttpStatusCode.Created, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "Create a new user in the system";
        Description = "Create a new user in the system";
        Response(201, "User was successfully created");
    }
}

file sealed class RequestValidator : Validator<CreateUserCommand>
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