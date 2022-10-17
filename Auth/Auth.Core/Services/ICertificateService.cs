namespace Auth.Core.Services;

public interface ICertificateService
{
    ValueTask<Certificate> GenerateAsync(string id, string scope = "global", CancellationToken cancellationToken = default);
    
}