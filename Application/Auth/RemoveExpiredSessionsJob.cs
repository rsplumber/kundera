using Core.Auth.Sessions;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredSessionsJob : IJob
{
    private readonly IExpiredSessionsService _expiredSessionsService;

    public RemoveExpiredSessionsJob(IExpiredSessionsService expiredSessionsService)
    {
        _expiredSessionsService = expiredSessionsService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _expiredSessionsService.DeleteAsync(context.CancellationToken);
    }
}