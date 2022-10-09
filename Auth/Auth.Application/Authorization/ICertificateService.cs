namespace Auth.Application.Authorization;

public interface ICertificateService
{
    ValueTask<Certificate> GenerateAsync(string id, string scope = "global", CancellationToken cancellationToken = default);
    
}