namespace Authorization.Application;

public interface ICertificateService
{
    Task<Certificate> GenerateAsync(string oneTimeToken, CancellationToken cancellationToken = default);
}