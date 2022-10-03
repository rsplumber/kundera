namespace Authorization.Application;

public interface ICertificateService
{
    Task<Certificate> GenerateAsync(string id, string scope = "global", CancellationToken cancellationToken = default);
    
}