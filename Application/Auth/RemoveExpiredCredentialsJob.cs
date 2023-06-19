using Core.Auth.Credentials;
using Quartz;

namespace Application.Auth;

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