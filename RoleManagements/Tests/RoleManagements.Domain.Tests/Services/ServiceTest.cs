using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Events;
using RoleManagements.Domain.Services.Exceptions;
using RoleManagements.Domain.Services.Types;

namespace RoleManagements.Domain.Tests.Services;

public class ServiceTest
{
    private readonly IServiceRepository _repository;
    private const string ServiceName = "FundManagement";

    public ServiceTest()
    {
        _repository = new ServiceRepository();
    }

    [Fact]
    public async Task service_creation()
    {
        var service = await Service.CreateAsync(ServiceName, _repository);
        await _repository.CreateAsync(service);
        Assert.NotNull(service);
        Assert.Equal(ServiceName, service.Id.Value);
    }

    [Fact]
    public async Task service_creation_event()
    {
        var service = await Service.CreateAsync("LoanManagement", _repository);
        Assert.Contains(service.DomainEvents!, de => de.GetType() == typeof(ServiceCreatedEvent));
    }

    [Fact]
    public async Task service_duplicate_creation_fail()
    {
        await Assert.ThrowsAsync<ServiceAlreadyExistsException>(async () =>
        {
            await Service.CreateAsync(ServiceName, _repository);
        });
    }

    [Fact]
    public async Task service_toggle_status()
    {
        var id = ServiceId.From(ServiceName);
        var service = await _repository.FindAsync(id);
        Assert.Equal(ServiceStatus.Active, service!.Status);
        service.DeActivate();
        Assert.Equal(ServiceStatus.DeActive, service.Status);
        service.Activate();
        Assert.Equal(ServiceStatus.Active, service.Status);
    }

    [Fact]
    public async Task service_status_changed_event()
    {
        var service = await Service.CreateAsync("Statuser", _repository);
        service.DeActivate();
        Assert.Contains(service.DomainEvents!, de => de.GetType() == typeof(ServiceStatusChangedEvent));
    }
}