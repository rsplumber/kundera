using RoleManagements.Domain.Roles;
using RoleManagements.Domain.Tests.Roles;
using RoleManagements.Domain.UserRoles;
using RoleManagements.Domain.UserRoles.Events;
using RoleManagements.Domain.UserRoles.Exceptions;

namespace RoleManagements.Domain.Tests.UserRoles;

public class UserRoleTest
{
    [Fact]
    public async Task user_role_creation()
    {
        var userId = UserId.From(Guid.Empty);
        var repository = new UserRolesRepository();
        var userRole = await UserRole.CreateAsync(userId, repository);
        await repository.CreateAsync(userRole);
        Assert.NotNull(userRole);
        Assert.Equal(userId.Value, userRole.Id.Value);
    }

    [Fact]
    public async Task user_role_event()
    {
        var userId = UserId.From(Guid.Empty);
        var repository = new UserRolesRepository();
        var userRole = await UserRole.CreateAsync(userId, repository);
        await repository.CreateAsync(userRole);
        Assert.Contains(userRole.DomainEvents!, de => de.GetType() == typeof(UserRoleCreatedEvent));
    }

    [Fact]
    public async Task user_role_duplicate_creation_fail()
    {
        var userId = UserId.From(Guid.Empty);
        var repository = new UserRolesRepository();
        var userRole = await UserRole.CreateAsync(userId, repository);
        await repository.CreateAsync(userRole);
        await Assert.ThrowsAsync<UserRoleAlreadyExistsException>(async () => { await UserRole.CreateAsync(userId, repository); });
    }

    [Fact]
    public async Task user_role_add_role()
    {
        var userId = UserId.From(Guid.Empty);
        var repository = new UserRolesRepository();
        var userRole = await UserRole.CreateAsync(userId, repository);

        var roleRepository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", roleRepository);
        userRole.AddRole(role.Id);
        Assert.Equal(1, userRole.Roles.Count);
    }

    [Fact]
    public async Task user_role_remove_role()
    {
        var userId = UserId.From(Guid.Empty);
        var repository = new UserRolesRepository();
        var userRole = await UserRole.CreateAsync(userId, repository);

        var roleRepository = new RoleRepository();
        var role = await Role.CreateAsync("Admin", roleRepository);
        userRole.AddRole(role.Id);
        Assert.Equal(1, userRole.Roles.Count);
        userRole.RemoveRole(role.Id);
        Assert.Equal(0, userRole.Roles.Count);
    }
}