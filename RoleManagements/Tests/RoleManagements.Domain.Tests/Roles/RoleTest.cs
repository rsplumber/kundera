using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Events;
using RoleManagements.Domain.Roles.Exceptions;
using RoleManagements.Domain.Tests.Permissions;

namespace RoleManagements.Domain.Tests.Roles;

public class RoleTest
{
    [Fact]
    public async Task role_creation()
    {
        var repository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", repository);
        Assert.NotNull(role);
        Assert.Equal("Admin", role.Id.Value);
    }

    [Fact]
    public async Task role_creation_event()
    {
        var repository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", repository);
        Assert.Contains(role.DomainEvents!, de => de.GetType() == typeof(RoleCreatedEvent));
    }

    [Fact]
    public async Task role_duplicate_creation_fail()
    {
        var name = "Admin";
        var repository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", repository);
        await repository.AddAsync(role);
        await Assert.ThrowsAsync<RoleAlreadyExistsException>(async () => { await Role.CreateAsync(name, repository); });
    }

    [Fact]
    public async Task role_add_meta()
    {
        var repository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", repository);
        role!.AddMeta("title", "hello");
        Assert.Equal(1, role.Meta.Count);
        Assert.NotNull(role.GetMetaValue("title"));
        Assert.Equal("hello", role.GetMetaValue("title"));
    }

    [Fact]
    public async Task role_remove_meta()
    {
        var repository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", repository);
        role!.AddMeta("title", "hello");
        Assert.Equal(1, role!.Meta.Count);
        role.RemoveMeta("title");
        Assert.Equal(0, role.Meta.Count);
    }

    [Fact]
    public async Task role_add_permission()
    {
        var permissionRepository = new PermissionRepository();
        var permission = await Permission.CreateAsync("create_loan", permissionRepository);

        var repository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", repository);
        await role.AddPermissionAsync(permission.Id, permissionRepository);
        Assert.Equal(1, role.Permissions.Count);
        Assert.True(role.HasPermission(permission.Id));
    }

    [Fact]
    public async Task role_remove_permission()
    {
        var permissionRepository = new PermissionRepository();
        var permission = await Permission.CreateAsync("create_loan", permissionRepository);

        var repository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", repository);
        await role.AddPermissionAsync(permission.Id, permissionRepository);
        Assert.Equal(1, role.Permissions.Count);

        role.RemovePermission(permission.Id);
        Assert.Equal(0, role.Permissions.Count);
        Assert.False(role.HasPermission(permission.Id));
    }
}