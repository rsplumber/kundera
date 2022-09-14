using System.Net;
using Authorization.Application;
using Authorization.Domain;
using Authorization.Domain.Types;
using Microsoft.Extensions.Options;

namespace Authorization.Infrastructure;

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
        await _sessionRepository.CreateAsync(session, cancellationToken);
    }

    public async Task UpdateAsync(Token token, DateTime lastUsageDate, IPAddress ipAddress, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);
        if (session is null) return;
        session.UpdateUsage(lastUsageDate, ipAddress);
        await _sessionRepository.UpdateAsync(session, cancellationToken);
    }

    public async Task DeleteAsync(Token token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async Task<SessionModel?> GetAsync(Token token, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);
        if (session is null) return null;
        return new SessionModel
        {
            Scope = session.Scope,
            UserId = session.UserId,
            ExpireDateUtc = session.ExpireDate,
            LastIpAddress = session.LastIpAddress,
            LastUsageDateUtc = session.LastUsageDate
        };
    }

    public async Task<IEnumerable<SessionModel>> GetAllAsync(string? scope = null, Guid? userId = null, DateOnly? expireDate = null, DateOnly? lastUsageDateUtc = null, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}