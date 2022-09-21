using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Events;
using RoleManagements.Domain.Permissions.Exceptions;

namespace RoleManagements.Domain.Tests.Permissions;

public class PermissionTest
{
    [Fact]
    public async Task permission_creation()
    {
        var repository = new PermissionRepository();
        var permission = await Permission.CreateAsync("create_loan", repository);
        Assert.NotNull(permission);
        Assert.Equal("create_loan", permission.Id.Value);
    }

    [Fact]
    public async Task permission_creation_event()
    {
        var repository = new PermissionRepository();
        var permission = await Permission.CreateAsync("create_loan", repository);
        Assert.Contains(permission.DomainEvents!, de => de.GetType() == typeof(PermissionCreatedEvent));
    }

    [Fact]
    public async Task permission_duplicate_creation_fail()
    {
        var name = "create_loan";
        var repository = new PermissionRepository();
        var permission = await Permission.CreateAsync("create_loan", repository);
        await repository.AddAsync(permission);
        await Assert.ThrowsAsync<PermissionAlreadyExistsException>(async () => { await Permission.CreateAsync(name, repository); });
    }

    [Fact]
    public async Task permission_add_meta()
    {
        var repository = new PermissionRepository();
        var permission = await Permission.CreateAsync("create_loan", repository);
        permission!.AddMeta("title", "hello");
        Assert.Equal(1, permission.Meta.Count);
        Assert.NotNull(permission.GetMetaValue("title"));
        Assert.Equal("hello", permission.GetMetaValue("title"));
    }

    [Fact]
    public async Task permission_remove_meta()
    {
        var repository = new PermissionRepository();
        var permission = await Permission.CreateAsync("create_loan", repository);
        permission!.AddMeta("title", "hello");
        Assert.Equal(1, permission!.Meta.Count);
        permission.RemoveMeta("title");
        Assert.Equal(0, permission.Meta.Count);
    }
}