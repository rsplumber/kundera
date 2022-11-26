using Core.Domains.Sessions;
using Core.Services;

namespace Application.Auth;

internal sealed class SessionManagement : ISessionManagement
{
    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionRepository _sessionRepository;
    private readonly SessionOptions _sessionOptions;

    public SessionManagement(ISessionRepository sessionRepository, IOptions<SessionOptions> sessionOptions, ISessionFactory sessionFactory)
    {
        _sessionRepository = sessionRepository;
        _sessionFactory = sessionFactory;
        _sessionOptions = sessionOptions.Value;
    }

    public async Task SaveAsync(Certificate certificate, Guid userId, Guid scopeId, CancellationToken cancellationToken = default)
    {
        var (token, refreshToken) = certificate;
        var expiresAt = DateTime.UtcNow.AddMinutes(_sessionOptions.ExpireInMinutes);
        await _sessionFactory.CreateAsync(
            Token.From(token),
            Token.From(refreshToken),
            scopeId,
            userId,
            expiresAt);
    }

    public async Task DeleteAsync(Token token, CancellationToken cancellationToken = default)
    {
        await _sessionRepository.DeleteAsync(token, cancellationToken);
    }

    public async Task<Session?> GetAsync(Token token, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.FindAsync(token, cancellationToken);

        if (session is null) return null;

        if (!Expired())
        {
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
}