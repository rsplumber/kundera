using RoleManagements.Domain.Services;
using RoleManagements.Domain.Services.Events;
using RoleManagements.Domain.Services.Exceptions;
using RoleManagements.Domain.Services.Types;

namespace RoleManagements.Domain.Tests.Services;

public class ServiceTest
{
    private const string ServiceName = "FundManagement";


    [Fact]
    public async Task service_creation()
    {
        var repository = new ServiceRepository();
        var service = await Service.CreateAsync(ServiceName, repository);
        await repository.AddAsync(service);
        Assert.NotNull(service);
        Assert.Equal(ServiceName, service.Id.Value);
    }

    [Fact]
    public async Task service_creation_event()
    {
        var repository = new ServiceRepository();
        var service = await Service.CreateAsync("LoanManagement", repository);
        Assert.Contains(service.DomainEvents!, de => de.GetType() == typeof(ServiceCreatedEvent));
    }

    [Fact]
    public async Task service_duplicate_creation_fail()
    {
        var name = "LoanManagement";
        var repository = new ServiceRepository();
        var service = await Service.CreateAsync(name, repository);
        await repository.AddAsync(service);
        await Assert.ThrowsAsync<ServiceAlreadyExistsException>(async () => { await Service.CreateAsync(name, repository); });
    }

    [Fact]
    public async Task service_toggle_status()
    {
        var repository = new ServiceRepository();
        var service = await Service.CreateAsync("LoanManagement", repository);
        Assert.Equal(ServiceStatus.Active, service.Status);
        service.DiActivate();
        Assert.Equal(ServiceStatus.DeActive, service.Status);
        service.Activate();
        Assert.Equal(ServiceStatus.Active, service.Status);
    }

    [Fact]
    public async Task service_status_changed_event()
    {
        var repository = new ServiceRepository();
        var service = await Service.CreateAsync("LoanManagement", repository);
        service.DiActivate();
        Assert.Contains(service.DomainEvents!, de => de.GetType() == typeof(ServiceStatusChangedEvent));
    }
}