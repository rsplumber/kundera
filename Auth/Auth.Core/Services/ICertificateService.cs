namespace Auth.Core.Services;

public interface ICertificateService
{
    Task<Certificate> GenerateAsync(Guid userId, Guid scopeId, CancellationToken cancellationToken = default);
}