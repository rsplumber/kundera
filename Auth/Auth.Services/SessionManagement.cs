using System.Net;
using Auth.Core;
using Auth.Core.Services;
using Microsoft.Extensions.Options;

namespace Auth.Services;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionRepository _sessionRepository;
    private readonly SessionOptions _sessionOptions;

    public SessionManagement(ISessionRepository sessionRepository, IOptions<SessionOptions> sessionOptions)
    {
        _sessionRepository = sessionRepository;
        _sessionOptions = sessionOptions.Value;
    }

    public async Task SaveAsync(Certificate certificate, Guid userId, Guid scopeId, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var (token, refreshToken) = certificate;
        var expiresAt = DateTime.UtcNow.AddMinutes(_sessionOptions.ExpireInMinutes);
        var session = Session.Create(token,
            refreshToken,
            scopeId,
            userId,
            expiresAt,
            ipAddress);

        await _sessionRepository.AddAsync(session, cancellationToken);
    }

    public async Task DeleteAsync(Token token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async Task<Session?> GetAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);

        if (session is null) return null;

        if (!Expired())
        {
            await UpdateAsync(session, ipAddress, cancellationToken);
            return session;
        }

        await DeleteAsync(token, cancellationToken);

        return null;

        bool Expired() => DateTime.UtcNow >= session.ExpiresAt;
    }

    public async Task<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(userId, cancellationToken);
    }

    private async Task UpdateAsync(Session session, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        session.UpdateUsage(DateTime.UtcNow, ipAddress);
        await _sessionRepository.UpdateAsync(session, cancellationToken);
    }
}