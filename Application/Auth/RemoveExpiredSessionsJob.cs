using Core.Domains.Auth.Sessions;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredSessionsJob : IJob
{
    private readonly ISessionRepository _sessionRepository;
    private int _afterExpireInMinutes;

    public RemoveExpiredSessionsJob(ISessionRepository sessionRepository, IConfiguration configuration)
    {
        _sessionRepository = sessionRepository;
        var removeExpiredSessionsAfterInMinutesValue = configuration.GetSection("RemoveExpiredSessionsAfterInMinutes").Value ??
                                                       throw new ArgumentNullException("Enter RemoveExpiredSessionsAfterInMinutes in appsettings.json");
        _afterExpireInMinutes = int.Parse(removeExpiredSessionsAfterInMinutesValue);
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _sessionRepository.DeleteExpiredAsync(_afterExpireInMinutes, context.CancellationToken);
    }
}