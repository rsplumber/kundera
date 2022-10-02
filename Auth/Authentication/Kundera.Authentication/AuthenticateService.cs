using System.Net;

namespace Kundera.Authentication;

public class AuthenticateService : IAuthenticateService
{
    public async Task<Certificate> AuthenticateAsync(string uniqueIdentifier, string password, string? scope = null, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}