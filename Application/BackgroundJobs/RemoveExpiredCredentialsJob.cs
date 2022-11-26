using Core.Domains.Credentials;
using Quartz;

namespace Application.BackgroundJobs;

internal sealed class RemoveExpiredCredentialsJob : IJob
{
    private readonly ICredentialRepository _credentialRepository;


    public RemoveExpiredCredentialsJob(ICredentialRepository credentialRepository)
    {
        _credentialRepository = credentialRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _credentialRepository.DeleteExpiredAsync(context.CancellationToken);
    }
}