using Core.Auth.Credentials;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredSessionsJob : IJob
{
    private readonly IExpiredCredentialsService _expiredCredentialsService;

    public RemoveExpiredSessionsJob(IExpiredCredentialsService expiredCredentialsService)
    {
        _expiredCredentialsService = expiredCredentialsService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _expiredCredentialsService.DeleteAsync(context.CancellationToken);
    }
}