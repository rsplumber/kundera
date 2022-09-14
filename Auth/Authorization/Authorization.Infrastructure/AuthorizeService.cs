using Authorization.Abstractions;

namespace Authorization.Infrastructure;

internal sealed class AuthorizeService : IAuthorizeService
{
    public async ValueTask<bool> AuthorizeAsync(string token, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}