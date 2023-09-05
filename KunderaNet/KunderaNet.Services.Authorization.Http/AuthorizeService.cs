using System.Net;
using System.Net.Http.Json;
using KunderaNet.Services.Authorization.Abstractions;

namespace KunderaNet.Services.Authorization.Http;

internal sealed class AuthorizeService : IAuthorizeService
{
    private const string ClientKey = "default";
    private const string AuthorizePermissionUrl = "api/v1/authorize/permission";
    private const string AuthorizeRoleUrl = "api/v1/authorize/role";
    private readonly IHttpClientFactory _clientFactory;

    public AuthorizeService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }


    public async Task<(int, AuthorizedResponse?)> AuthorizePermissionAsync(string token,
        IEnumerable<string> permissions,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.CreateClient(ClientKey);
        if (headers is not null)
        {
            foreach (var (key, value) in headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
            }
        }
        
        var result = await client.PostAsJsonAsync(AuthorizePermissionUrl, new
        {
            Authorization = token,
            Actions = permissions
        }, cancellationToken);
        return await MapResponseAsync(result, cancellationToken);
    }

    public async Task<(int, AuthorizedResponse?)> AuthorizeRoleAsync(string token,
        IEnumerable<string> roles,
        Dictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default)
    {
        var client = _clientFactory.CreateClient(ClientKey);
        if (headers is not null)
        {
            foreach (var (key, value) in headers)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
            }
        }

        var result = await client.PostAsJsonAsync(AuthorizeRoleUrl, new
        {
            Authorization = token,
            Roles = roles
        }, cancellationToken);
        return await MapResponseAsync(result, cancellationToken);
    }

    private static async Task<(int, AuthorizedResponse?)> MapResponseAsync(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken = default)
    {
        if (!httpResponseMessage.IsSuccessStatusCode) return ((int)httpResponseMessage.StatusCode, null);
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<AuthorizedResponse>(cancellationToken: cancellationToken);
        return response is null ? ((int)HttpStatusCode.Unauthorized, null) : ((int)httpResponseMessage.StatusCode, response);
    }
}