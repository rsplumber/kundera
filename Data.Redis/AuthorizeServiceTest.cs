using System.Net;
using Core.Services;
using Managements.Data.Auth.Sessions;
using Redis.OM;

namespace Managements.Data;

internal sealed class AuthorizeServiceTest : IAuthorizeService
{
    private readonly RedisConnectionProvider _dbProvider;

    public AuthorizeServiceTest(RedisConnectionProvider dbProvider)
    {
        _dbProvider = dbProvider;
    }

    public async Task<Guid> AuthorizeAsync(string token, string action, string serviceSecret, IPAddress? ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _dbProvider.RedisCollection<SessionDataModel>(false).FindByIdAsync(token);
        if (session is null)
        {
            throw new UnAuthorizedException();
        }


        return session.UserId;
    }
}