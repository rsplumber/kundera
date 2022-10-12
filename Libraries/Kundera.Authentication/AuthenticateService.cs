using System.Net;
using RestSharp;

namespace Kundera.Authentication;

internal sealed class AuthenticateService : IAuthenticateService
{
    private readonly AuthenticationSettings _authenticationSettings;

    public async Task<Certificate?> AuthenticateAsync(string username, string password, string type = "default", string scope = "global", IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var body = new
        {
            username,
            password,
            type,
            scope,
            ipAddress
        };
        var request = new RestRequest(_authenticationSettings.Url + "/authenticate")
            .AddJsonBody(body);
        var client = new RestClient();
        return await client.PostAsync<Certificate>(request, cancellationToken);
    }
}