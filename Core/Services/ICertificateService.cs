using Core.Domains.Scopes.Types;
using Core.Domains.Users.Types;

namespace Core.Services;

public interface ICertificateService
{
    Task<Certificate> GenerateAsync(UserId userId, ScopeId scopeId, CancellationToken cancellationToken = default);
}