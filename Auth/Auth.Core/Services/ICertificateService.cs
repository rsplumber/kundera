namespace Auth.Core.Services;

public interface ICertificateService
{
    Task<Certificate> GenerateAsync(string id, string scope = "global", CancellationToken cancellationToken = default);
}