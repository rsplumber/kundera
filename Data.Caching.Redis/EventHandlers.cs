using DotNetCore.CAP;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace Data.Caching.Redis;

internal sealed class EventHandlers : ICapSubscribe
{
    private readonly IDistributedCache _cacheService;
    private readonly IServiceProvider _serviceProvider;
    private readonly string? _dbType;
    private const string QueueGroupName = "kundera.caching.queue";

    public EventHandlers(IDistributedCache cacheService, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _cacheService = cacheService;
        _serviceProvider = serviceProvider;
        _dbType = configuration.GetSection("Caching:Type").Value;
    }


    [CapSubscribe("kundera.scopes.*", Group = QueueGroupName)]
    public Task HandleScopesAsync(dynamic data)
    {
        throw new NotImplementedException();
    }

    [CapSubscribe("kundera.users.*", Group = QueueGroupName)]
    public Task HandleUsersAsync(dynamic data)
    {
        throw new NotImplementedException();
    }

    [CapSubscribe("kundera.credentials.*", Group = QueueGroupName)]
    public Task HandleCredentialsAsync(dynamic data)
    {
        throw new NotImplementedException();
    }

    [CapSubscribe("kundera.services.*", Group = QueueGroupName)]
    public Task HandleServicesAsync(dynamic data)
    {
        throw new NotImplementedException();
    }

    [CapSubscribe("kundera.groups.*", Group = QueueGroupName)]
    public Task HandleGroupsAsync(dynamic data)
    {
        throw new NotImplementedException();
    }

    [CapSubscribe("kundera.roles.*", Group = QueueGroupName)]
    public Task HandleRolesAsync(dynamic data)
    {
        throw new NotImplementedException();
    }

    [CapSubscribe("kundera.permissions.*", Group = QueueGroupName)]
    public Task HandlePermissionsAsync(dynamic data)
    {
        throw new NotImplementedException();
    }
}