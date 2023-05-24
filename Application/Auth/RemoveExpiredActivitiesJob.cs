using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Quartz;

namespace Application.Auth;

internal sealed class RemoveExpiredActivitiesJob : IJob
{
    private readonly IAuthorizationActivityRepository _authorizationActivityRepository;
    private readonly IAuthenticationActivityRepository _authenticationActivityRepository;

    public RemoveExpiredActivitiesJob(IAuthorizationActivityRepository authorizationActivityRepository, IAuthenticationActivityRepository authenticationActivityRepository)
    {
        _authorizationActivityRepository = authorizationActivityRepository;
        _authenticationActivityRepository = authenticationActivityRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _authorizationActivityRepository.RemoveExpiredActivitiesAsync(context.CancellationToken);
        await _authenticationActivityRepository.RemoveExpiredActivitiesAsync(context.CancellationToken);
    }
}