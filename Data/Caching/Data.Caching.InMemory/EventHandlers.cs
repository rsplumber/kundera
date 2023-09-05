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
    public async Task HandleScopesAsync(dynamic data)
    {
    }

    [CapSubscribe("kundera.users.*", Group = QueueGroupName)]
    public async Task HandleUsersAsync(dynamic data)
    {
    }

    [CapSubscribe("kundera.credentials.*", Group = QueueGroupName)]
    public async Task HandleCredentialsAsync(dynamic data)
    {
    }

    [CapSubscribe("kundera.services.*", Group = QueueGroupName)]
    public async Task HandleServicesAsync(dynamic data)
    {
    }

    [CapSubscribe("kundera.groups.*", Group = QueueGroupName)]
    public async Task HandleGroupsAsync(dynamic data)
    {
    }

    [CapSubscribe("kundera.roles.*", Group = QueueGroupName)]
    public async Task HandleRolesAsync(dynamic data)
    {
    }

    [CapSubscribe("kundera.permissions.*", Group = QueueGroupName)]
    public async Task HandlePermissionsAsync(dynamic data)
    {
    }
}