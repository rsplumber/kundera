using System.Net;
using Auth.Application;
using Auth.Domain.Sessions;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionRepository _sessionRepository;
    private readonly SessionOptions _sessionOptions;

    public SessionManagement(ISessionRepository sessionRepository, IOptions<SessionOptions> sessionOptions)
    {
        _sessionRepository = sessionRepository;
        _sessionOptions = sessionOptions.Value;
    }

    public async Task SaveAsync(Certificate certificate, string scope, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var userId = Guid.Empty;
        var expireDate = DateTime.Now.AddMinutes(_sessionOptions.ExpireInMinutes);
        var (token, refreshToken) = certificate;
        var session = Session.Create(
            token,
            refreshToken,
            scope,
            userId,
            expireDate,
            ipAddress
        );
        await _sessionRepository.AddAsync(session, cancellationToken);
    }

    public async Task DeleteAsync(Token token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async Task<SessionModel?> GetAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);
        if (session is null) return null;
        await UpdateAsync(token, ipAddress, cancellationToken);
        return new SessionModel
        {
            Scope = session.Scope,
            UserId = session.UserId,
            ExpireDateUtc = session.ExpireDate,
            LastIpAddress = session.LastIpAddress,
            LastUsageDateUtc = session.LastUsageDate
        };
    }

    public async Task<IEnumerable<SessionModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task UpdateAsync(Token token, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);
        if (session is null) return;
        session.UpdateUsage(DateTime.UtcNow, ipAddress);
        await _sessionRepository.UpdateAsync(session, cancellationToken);
    }
}