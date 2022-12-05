using Core.Services;
using FastEndpoints;

namespace Web.Api.Endpoints.V1.Auth.Authorizations;

internal sealed class AuthorizeEndpointTest : EndpointWithoutRequest<Guid>
{
    private readonly IAuthorizeService _authorizeService;

    public AuthorizeEndpointTest(IAuthorizeService authorizeService)
    {
        _authorizeService = authorizeService;
    }

    public override void Configure()
    {
        Get("authorize/permission/test");
        AllowAnonymous();
        Version(1);
        ResponseCache(5);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = await _authorizeService.AuthorizePermissionAsync(
            "099EB5DC85E7995A01641EF3FF3B30393962FFD4479400F7E8337A58B544362F1E34E24EFAF811C2CC122F3F198F7A44",
            new List<string>() {"groups_list"},
            "EF8EC7F4A09BF3600F09B050905078CDA75403E7FF288B033C9D8D72AE65B30DEDE72E47B5D87774427DB8B9DA15DA2B9D1A5BF0020597DF73A03F570DB91343",
            HttpContext.Connection.LocalIpAddress,
            ct);

        await SendOkAsync(userId, ct);
    }
}