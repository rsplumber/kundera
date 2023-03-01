using Core.Domains.Auth.Sessions;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredSessionsJob : IJob
{
    private readonly ISessionRepository _sessionRepository;

    public RemoveExpiredSessionsJob(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _sessionRepository.DeleteExpiredAsync(context.CancellationToken);
    }
}