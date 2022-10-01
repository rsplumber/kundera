namespace Authorization.Application;

public interface ICertificateService
{
    Task<Certificate> GenerateAsync(string id, CancellationToken cancellationToken = default);
}