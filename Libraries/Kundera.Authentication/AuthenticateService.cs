using System.Net;

namespace Kundera.Authentication;

internal sealed class AuthenticateService : IAuthenticateService
{
    public async Task<Certificate> AuthenticateAsync(string username, string password, string type = "default", string scope = "global", IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}