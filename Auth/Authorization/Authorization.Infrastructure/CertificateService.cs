using Authorization.Application;

namespace Authorization.Infrastructure;

internal sealed class CertificateService : ICertificateService
{
    public async Task<Certificate> GenerateAsync(string oneTimeToken, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}