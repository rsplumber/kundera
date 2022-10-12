using System.Net;
using Microsoft.Extensions.Options;
using RestSharp;

namespace Kundera.Authentication;

internal sealed class CredentialService : ICredentialService
{
    private readonly AuthenticationSettings _authenticationSettings;

    public CredentialService(IOptions<AuthenticationSettings> authenticationSettings)
    {
        _authenticationSettings = authenticationSettings.Value;
    }

    public async Task<bool> CreateAsync(string username, string password, Guid userId, string type, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(_authenticationSettings.Url + $@"/users/{userId}/credentials")
            .AddJsonBody(new
            {
                username, password, type
            });
        var client = new RestClient();
        var response = await client.PostAsync(request, cancellationToken);
        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> CreateOneTimeAsync(string username, string password, Guid userId, string type, int expirationTimeInSeconds = 0, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(_authenticationSettings.Url + $@"/users/{userId}/credentials/one-time")
            .AddJsonBody(new
            {
                username, password, type, expirationTimeInSeconds
            });
        var client = new RestClient();
        var response = await client.PostAsync(request, cancellationToken);
        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> CreateTimePeriodicAsync(string username, string password, Guid userId, string type, int expirationTimeInSeconds, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(_authenticationSettings.Url + $@"/users/{userId}/credentials/time-periodic")
            .AddJsonBody(new
            {
                username, password, type, expirationTimeInSeconds
            });
        var client = new RestClient();
        var response = await client.PostAsync(request, cancellationToken);
        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(string uniqueIdentifier, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(_authenticationSettings.Url + $@"/credentials/{uniqueIdentifier}");
        var client = new RestClient();
        var response = await client.DeleteAsync(request, cancellationToken);
        return response.StatusCode == HttpStatusCode.OK;
    }
}