using Data.Abstractions.Users;
using FastEndpoints;
using KunderaNet.Authorization;
using Mediator;

namespace Application.Users.Me.Roles;

file sealed class Endpoint : EndpointWithoutRequest<List<UserRoleResponse>>
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public Endpoint(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public override void Configure()
    {
        Get("users/me/roles");
        Permissions("users_me_roles");
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var response = await _mediator.Send(new UsersRolesQuery
        {
            UserId = _currentUserService.User().Id
        }, ct);

        await SendOkAsync(response, ct);
    }
}

file sealed class EndpointSummary : Summary<Endpoint>
{
    public EndpointSummary()
    {
        Summary = "User role list";
        Description = "User list";
        Response<List<UserRoleResponse>>(200, "Users was successfully received");
    }
}