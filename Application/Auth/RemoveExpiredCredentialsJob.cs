using Core.Auth.Credentials;
using Core.Auth.Sessions;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredCredentialsJob : IJob
{

    private readonly IExpiredCredentialsService _expiredCredentialsService;

    public RemoveExpiredCredentialsJob(IExpiredCredentialsService expiredCredentialsService)
    {
        _expiredCredentialsService = expiredCredentialsService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _expiredCredentialsService.DeleteAsync(context.CancellationToken);
    }
}