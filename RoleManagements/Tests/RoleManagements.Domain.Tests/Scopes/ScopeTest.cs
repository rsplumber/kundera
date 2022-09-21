using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Scopes;
using RoleManagements.Domain.Scopes.Events;
using RoleManagements.Domain.Scopes.Exceptions;
using RoleManagements.Domain.Services;
using RoleManagements.Domain.Tests.Roles;
using RoleManagements.Domain.Tests.Services;

namespace RoleManagements.Domain.Tests.Scopes;

public class ScopeTest
{
    [Fact]
    public async Task scope_creation()
    {
        var scopeName = "application";
        var repository = new ScopeRepository();
        var scope = await Scope.CreateAsync(scopeName, repository);
        Assert.IsType<Scope>(scope);
        Assert.NotNull(scope);
        Assert.Equal(scopeName, scope.Id.Value);
    }

    [Fact]
    public async Task scope_creation_event()
    {
        var scopeName = "application";
        var repository = new ScopeRepository();
        var scope = await Scope.CreateAsync(scopeName, repository);
        Assert.Contains(scope.DomainEvents!, de => de.GetType() == typeof(ScopeCreatedEvent));
    }

    [Fact]
    public async Task scope_duplicate_creation_fail()
    {
        var scopeName = "application";
        var repository = new ScopeRepository();
        var scope = await Scope.CreateAsync(scopeName, repository);
        await repository.AddAsync(scope);
        await Assert.ThrowsAsync<ScopeAlreadyExistsException>(async () => { await Scope.CreateAsync(scopeName, repository); });
    }

    [Fact]
    public async Task scope_add_role()
    {
        var scopeName = "application";
        var repository = new ScopeRepository();
        var scope = await Scope.CreateAsync(scopeName, repository);

        var roleRepository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", roleRepository);
        scope.AddRole(role.Id);
        Assert.Equal(1, scope.Roles.Count);
        Assert.Contains(scope.Roles, id => id == role.Id);
    }


    [Fact]
    public async Task scope_remove_role()
    {
        var scopeName = "application";
        var repository = new ScopeRepository();
        var scope = await Scope.CreateAsync(scopeName, repository);

        var roleRepository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", roleRepository);
        scope.AddRole(role.Id);
        Assert.Equal(1, scope.Roles.Count);
        scope.RemoveRole(role.Id);
        Assert.Equal(0, scope.Roles.Count);
    }

    [Fact]
    public async Task scope_add_service()
    {
        var scopeName = "application";
        var repository = new ScopeRepository();
        var scope = await Scope.CreateAsync(scopeName, repository);

        var serviceRepository = new ServiceRepository();
        var service = await Service.CreateAsync("Admin", serviceRepository);
        scope.AddService(service.Id);
        Assert.Equal(1, scope.Services.Count);
        Assert.Contains(scope.Services, id => id == service.Id);
    }


    [Fact]
    public async Task scope_remove_service()
    {
        var scopeName = "application";
        var repository = new ScopeRepository();
        var scope = await Scope.CreateAsync(scopeName, repository);

        var serviceRepository = new ServiceRepository();
        var service = await Service.CreateAsync("Admin", serviceRepository);
        scope.AddService(service.Id);
        Assert.Equal(1, scope.Services.Count);
        Assert.Contains(scope.Services, id => id == service.Id);
        scope.RemoveService(service.Id);
        Assert.Equal(0, scope.Services.Count);
    }
}