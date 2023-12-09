using DotNetCore.CAP;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace Data.Caching.InMemory;

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
    public Task HandleScopes(dynamic data)
    {
        return Task.CompletedTask;
    }

    [CapSubscribe("kundera.users.*", Group = QueueGroupName)]
    public Task HandleUsers(dynamic data)
    {
        return Task.CompletedTask;
    }

    [CapSubscribe("kundera.credentials.*", Group = QueueGroupName)]
    public Task HandleCredentials(dynamic data)
    {
        return Task.CompletedTask;
    }

    [CapSubscribe("kundera.services.*", Group = QueueGroupName)]
    public Task HandleServices(dynamic data)
    {
        return Task.CompletedTask;
    }

    [CapSubscribe("kundera.groups.*", Group = QueueGroupName)]
    public Task HandleGroups(dynamic data)
    {
        return Task.CompletedTask;
    }

    [CapSubscribe("kundera.roles.*", Group = QueueGroupName)]
    public Task HandleRoles(dynamic data)
    {
        return Task.CompletedTask;
    }

    [CapSubscribe("kundera.permissions.*", Group = QueueGroupName)]
    public Task HandlePermissions(dynamic data)
    {
        return Task.CompletedTask;
    }
}