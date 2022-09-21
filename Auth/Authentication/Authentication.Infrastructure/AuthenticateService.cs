using System.Net;
using Authentication.Application;
using Authentication.Domain;
using Authentication.Domain.Types;

namespace Authentication.Infrastructure;

internal class AuthenticateService : IAuthenticateService
{
    public async Task<OneTimeToken> AuthenticateAsync(UniqueIdentifier uniqueIdentifier, Password password, IPAddress? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}