using System.Net;
using Auth.Application.Authorization;
using Auth.Domain.Sessions;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure.Authorization;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionRepository _sessionRepository;
    private readonly SessionOptions _sessionOptions;

    public SessionManagement(ISessionRepository sessionRepository, IOptions<SessionOptions> sessionOptions)
    {
        _sessionRepository = sessionRepository;
        _sessionOptions = sessionOptions.Value;
    }

    public async ValueTask SaveAsync(Certificate certificate, Guid userId, string scope, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var (token, refreshToken) = certificate;
        var expiresAt = DateTime.UtcNow.AddMinutes(_sessionOptions.ExpireInMinutes);
        var session = Session.Create(token,
            refreshToken,
            scope,
            userId,
            expiresAt,
            ipAddress);

        await _sessionRepository.AddAsync(session, cancellationToken);
    }

    public async ValueTask DeleteAsync(Token token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async ValueTask<Session?> GetAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);

        if (session is null) return null;

        if (!Expired())
        {
            await UpdateAsync(token, ipAddress, cancellationToken);

            return session;
        }

        await DeleteAsync(token, cancellationToken);

        return null;

        bool Expired() => DateTime.UtcNow >= session.ExpiresAt;
    }

    public async ValueTask<IEnumerable<Session>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(cancellationToken);
    }

    public async ValueTask<IEnumerable<Session>> GetAllAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _sessionRepository.FindAsync(userId, cancellationToken);
    }

    private async ValueTask UpdateAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);

        if (session is null) return;

        session.UpdateUsage(DateTime.UtcNow, ipAddress);
        await _sessionRepository.UpdateAsync(session, cancellationToken);
    }
}