using System.Net;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Kundera.Authentication.Http;

internal sealed class AuthenticateService : IAuthenticateService
{
    private readonly AuthenticationSettings _authenticationSettings;

    public AuthenticateService(IOptions<AuthenticationSettings> authenticationSettings)
    {
        _authenticationSettings = authenticationSettings.Value;
    }

    public async Task<Certificate?> AuthenticateAsync(string username, string password, string type = "default", string scope = "global", IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(_authenticationSettings.BaseUrl + "/authenticate")
            .AddJsonBody(new
            {
                username,
                password,
                type,
                scope,
                ipAddress
            });
        var client = new RestClient();
        return await client.PostAsync<Certificate>(request, cancellationToken);
    }

    public async ValueTask<Certificate?> RefreshCertificateAsync(string token, string refreshToken, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(_authenticationSettings.BaseUrl + "/authenticate/refresh")
            .AddHeader("token", token)
            .AddJsonBody(new
            {
                refreshToken,
                ipAddress
            });
        var client = new RestClient();
        return await client.PostAsync<Certificate>(request, cancellationToken);
    }
}