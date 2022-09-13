using RoleManagements.Domain.Permissions;
using RoleManagements.Domain.Permissions.Types;
using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Roles.Events;
using RoleManagements.Domain.Roles.Exceptions;
using RoleManagements.Domain.Roles.Types;
using RoleManagements.Domain.Tests.Permissions;

namespace RoleManagements.Domain.Tests.Roles;

public class RoleTest
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private const string RoleName = "Admin";
    private const string PermissionName = "create_loan";

    public RoleTest()
    {
        _roleRepository = new RoleRepository();
        _permissionRepository = new PermissionRepository();
    }

    [Fact]
    public async Task role_creation()
    {
        var role = await Role.CreateAsync(RoleName, _roleRepository);
        Assert.NotNull(role);
        Assert.Equal(RoleName, role.Id.Value);
    }

    [Fact]
    public async Task role_creation_event()
    {
        var service = await Role.CreateAsync("Super", _roleRepository);
        Assert.Contains(service.DomainEvents!, de => de.GetType() == typeof(RoleCreatedEvent));
    }

    [Fact]
    public async Task role_duplicate_creation_fail()
    {
        await Assert.ThrowsAsync<RoleAlreadyExistsException>(async () => { await Role.CreateAsync(RoleName, _roleRepository); });
    }

    [Fact]
    public async Task role_add_meta()
    {
        var role = await _roleRepository.FindAsync(RoleId.From(RoleName));
        role!.AddMeta("title", "hello");
        Assert.Equal(1, role.Meta.Count);
        Assert.NotNull(role.GetMetaValue("title"));
        Assert.Equal("hello", role.GetMetaValue("title"));
    }

    [Fact]
    public async Task role_remove_meta()
    {
        var role = await _roleRepository.FindAsync(RoleId.From(RoleName));
        Assert.Equal(1, role!.Meta.Count);
        role.RemoveMeta("title");
        Assert.Equal(0, role.Meta.Count);
    }

    [Fact]
    public async Task role_add_permission()
    {
        var permission = await Permission.CreateAsync(PermissionName, _permissionRepository);
        await _permissionRepository.CreateAsync(permission);
        
        var role = await _roleRepository.FindAsync(RoleId.From(RoleName));
        role!.AddPermission(permission.Id);
        Assert.Equal(1, role.Permissions.Count);
        Assert.True(role.HasPermission(permission.Id));
    }

    [Fact]
    public async Task role_remove_permission()
    {
        var permissionId = PermissionId.From(PermissionName);
        var role = await _roleRepository.FindAsync(RoleId.From(RoleName));
        Assert.Equal(1, role!.Permissions.Count);
        role.RemovePermission(permissionId);
        Assert.Equal(0, role.Permissions.Count);
        Assert.False(role.HasPermission(permissionId));
    }
}