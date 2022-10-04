using System.Net;

namespace Kundera.Authentication;

internal sealed class AuthenticateService : IAuthenticateService
{
    public async Task<Certificate> AuthenticateAsync(string uniqueIdentifier, string password, string? scope = null, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}