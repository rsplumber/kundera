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
        var session = Session.Create(
            token,
            refreshToken,
            scope,
            userId,
            expiresAt,
            ipAddress
        );
        await _sessionRepository.AddAsync(session, cancellationToken);
    }

    public async ValueTask DeleteAsync(Token token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async ValueTask<SessionModel?> GetAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);
        if (session is null) return null;
        await UpdateAsync(token, ipAddress, cancellationToken);
        return new SessionModel
        {
            Scope = session.Scope,
            UserId = session.UserId,
            ExpiresAtUtc = session.ExpiresAt,
            LastIpAddress = session.LastIpAddress,
            LastUsageDateUtc = session.LastUsageDate,
            CreatedDateUtc = session.CreatedDate
        };
    }

    public async ValueTask<IEnumerable<SessionModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.FindAsync(cancellationToken);
        return sessions.Select(session => new SessionModel
        {
            Scope = session.Scope,
            UserId = session.UserId,
            ExpiresAtUtc = session.ExpiresAt,
            LastIpAddress = session.LastIpAddress,
            LastUsageDateUtc = session.LastUsageDate,
            CreatedDateUtc = session.CreatedDate
        });
    }

    private async ValueTask UpdateAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);
        if (session is null) return;
        session.UpdateUsage(DateTime.UtcNow, ipAddress);
        await _sessionRepository.UpdateAsync(session, cancellationToken);
    }
}