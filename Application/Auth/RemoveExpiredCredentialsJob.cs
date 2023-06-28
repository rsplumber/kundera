using Core.Auth.Sessions;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredCredentialsJob : IJob
{
    private readonly IExpiredSessionsService _expiredSessionsService;

    public RemoveExpiredCredentialsJob(IExpiredSessionsService expiredSessionsService)
    {
        _expiredSessionsService = expiredSessionsService;
    }


    public async Task Execute(IJobExecutionContext context)
    {
        await _expiredSessionsService.DeleteAsync(context.CancellationToken);
    }
}