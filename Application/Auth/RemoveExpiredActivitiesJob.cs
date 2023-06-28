using Core.Auth.Credentials;
using Core.Auth.Sessions;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredActivitiesJob : IJob
{
    private readonly IExpiredAuthorizationActivityService _expiredAuthorizationActivityService;
    private readonly IExpiredAuthenticationActivityService _expiredAuthenticationActivityService;

    public RemoveExpiredActivitiesJob(IExpiredAuthorizationActivityService expiredAuthorizationActivityService, IExpiredAuthenticationActivityService expiredAuthenticationActivityService)
    {
        _expiredAuthorizationActivityService = expiredAuthorizationActivityService;
        _expiredAuthenticationActivityService = expiredAuthenticationActivityService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _expiredAuthorizationActivityService.DeleteAsync(context.CancellationToken);
        await _expiredAuthenticationActivityService.DeleteAsync(context.CancellationToken);
    }
}