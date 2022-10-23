using System.Net;
using Auth.Core;
using Auth.Core.Services;
using Kite.Hashing;
using Managements.Domain;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.UserGroups;
using Managements.Domain.Users;
using Microsoft.Extensions.Configuration;

namespace Data.Seeder;

public class DefaultDataSeeder
{
    private readonly string _adminUsername;
    private readonly string _adminPassword;
    private readonly string _kunderaScopeSecret;
    private RoleId? _generatedSuperAdminRoleId;
    private ServiceId? _generatedKunderaServiceId;
    private RoleId? _generatedServiceAdminRoleId;

    private readonly IRoleRepository _roleRepository;
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICredentialService _credentialService;
    private readonly IHashService _hashService;


    public DefaultDataSeeder(IConfiguration configuration,
        IRoleRepository roleRepository,
        IUserGroupRepository userGroupRepository,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IScopeRepository scopeRepository,
        IServiceRepository serviceRepository,
        ICredentialService credentialService,
        IHashService hashService)
    {
        _roleRepository = roleRepository;
        _userGroupRepository = userGroupRepository;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
        _credentialService = credentialService;
        _hashService = hashService;
        _adminPassword = configuration.GetSection("AdminPassword").Value;
        _adminUsername = configuration.GetSection("AdminUsername").Value;
        _kunderaScopeSecret = configuration.GetSection("KunderaScopeSecret").Value;
    }


    public async Task SeedAsync()
    {
        await SeedPermissions();
        await SeedSuperAdminRoleAsync();
        await SeedAdministratorUserGroupAsync();
        await SeedUserAsync();
        await SeedServiceAsync();
        await SeedScopeAsync();
        await SeedCredentialAsync();

        await SeedServiceAdminRoleAsync();
        await SeedServiceManUserGroupAsync();
    }

    private async Task SeedPermissions()
    {
        var exists = await _permissionRepository.ExistsAsync("permissions_create");
        if (exists) return;

        await SeedPermissionPermissions();
        await SeedRolePermissions();
        await SeedServicePermissions();
        await SeedScopePermissions();
        await SeedUserGroupPermissions();
        await SeedUserPermissions();

        await SeedServiceManUserGroupAsync();
    }

    private async Task SeedPermissionPermissions()
    {
        await _permissionRepository.AddAsync(await Permission.FromAsync("permissions_create", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("permissions_list", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("permissions_get", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("permissions_delete", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("permissions_add_meta", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("permissions_remove_meta", _permissionRepository));
    }

    private async Task SeedRolePermissions()
    {
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_create", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_list", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_get", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_delete", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_add_permission", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_remove_permission", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_add_meta", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("roles_remove_meta", _permissionRepository));
    }

    private async Task SeedServicePermissions()
    {
        await _permissionRepository.AddAsync(await Permission.FromAsync("services_create", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("services_list", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("services_get", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("services_delete", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("services_activate", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("services_de-activate", _permissionRepository));
    }

    private async Task SeedScopePermissions()
    {
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_create", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_list", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_get", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_delete", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_active", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_de-active", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_add_service", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_remove_service", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_add_role", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("scopes_remove_role", _permissionRepository));
    }

    private async Task SeedUserGroupPermissions()
    {
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_create", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_list", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_get", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_delete", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_assign_role", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_revoke_role", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_set_parent", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_move_parent", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_remove_parent", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_enable", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user-groups_disable", _permissionRepository));
    }

    private async Task SeedUserPermissions()
    {
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_create", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_list", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_get", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_delete", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_add_username", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_remove_username", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_exist_username", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_assign_role", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_revoke_role", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_join_group", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_remove_group", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_activate", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_suspend", _permissionRepository));
        await _permissionRepository.AddAsync(await Permission.FromAsync("user_block", _permissionRepository));
    }

    private async Task SeedSuperAdminRoleAsync()
    {
        var roleExists = await _roleRepository.ExistsAsync(EntityBaseValues.SuperAdminRole);

        if (roleExists) return;

        var permissions = await _permissionRepository.FindAsync();

        var role = await Role.FromAsync(EntityBaseValues.SuperAdminRole, _roleRepository);
        foreach (var permission in permissions)
        {
            role.AddPermission(permission.Id);
        }

        await _roleRepository.AddAsync(role);
        _generatedSuperAdminRoleId = role.Id;
    }

    private async Task SeedAdministratorUserGroupAsync()
    {
        var role = await _roleRepository.FindAsync(_generatedSuperAdminRoleId);
        if (role is null) return;

        var userGroup = await _userGroupRepository.FindAsync(EntityBaseValues.AdministratorUserGroup);

        if (userGroup is not null) return;

        userGroup = await UserGroup.FromAsync(EntityBaseValues.AdministratorUserGroup, role.Id, _userGroupRepository);
        await _userGroupRepository.AddAsync(userGroup);
    }

    private async Task SeedServiceAdminRoleAsync()
    {
        var permissions = await Task.WhenAll(
            _permissionRepository.FindAsync("roles_create"),
            _permissionRepository.FindAsync("roles_create"),
            _permissionRepository.FindAsync("roles_get"),
            _permissionRepository.FindAsync("roles_delete"),
            _permissionRepository.FindAsync("roles_add_permission"),
            _permissionRepository.FindAsync("roles_remove_permission"),
            _permissionRepository.FindAsync("roles_add_meta"),
            _permissionRepository.FindAsync("roles_remove_meta"),
            _permissionRepository.FindAsync("permissions_create"),
            _permissionRepository.FindAsync("permissions_get"),
            _permissionRepository.FindAsync("permissions_delete"),
            _permissionRepository.FindAsync("permissions_add_meta"),
            _permissionRepository.FindAsync("permissions_remove_meta"),
            _permissionRepository.FindAsync("services_get"),
            _permissionRepository.FindAsync("services_activate"),
            _permissionRepository.FindAsync("services_de-activate")
        );

        var roleExists = await _roleRepository.ExistsAsync(EntityBaseValues.ServiceAdminRole);

        if (roleExists) return;


        var role = await Role.FromAsync(EntityBaseValues.ServiceAdminRole, _roleRepository);
        foreach (var permission in permissions)
        {
            role.AddPermission(permission.Id);
        }

        await _roleRepository.AddAsync(role);
        _generatedServiceAdminRoleId = role.Id;
    }

    private async Task SeedServiceManUserGroupAsync()
    {
        var role = await _roleRepository.FindAsync(_generatedServiceAdminRoleId);
        if (role is null) return;

        var userGroup = await _userGroupRepository.FindAsync(EntityBaseValues.ServiceManUserGroup);

        if (userGroup is not null) return;

        userGroup = await UserGroup.FromAsync(EntityBaseValues.ServiceManUserGroup, role.Id, _userGroupRepository);
        await _userGroupRepository.AddAsync(userGroup);
    }

    private async Task SeedUserAsync()
    {
        var userGroup = await _userGroupRepository.FindAsync(EntityBaseValues.AdministratorUserGroup);
        if (userGroup is null) return;

        var userExists = await _userRepository.ExistsAsync(_adminUsername);

        if (userExists) return;

        var user = await User.CreateAsync(_adminUsername, userGroup.Id, _userRepository);
        await _userRepository.AddAsync(user);
    }

    private async Task SeedServiceAsync()
    {
        var exists = await _serviceRepository.ExistsAsync(EntityBaseValues.KunderaServiceName);

        if (exists) return;

        var service = await Service.FromAsync(EntityBaseValues.KunderaServiceName, _hashService, _serviceRepository);
        await _serviceRepository.AddAsync(service);
        _generatedKunderaServiceId = service.Id;
    }

    private async Task SeedScopeAsync()
    {
        var exists = await _scopeRepository.ExistsAsync(EntityBaseValues.IdentityScopeName);

        if (exists) return;

        var service = await _serviceRepository.FindAsync(_generatedKunderaServiceId);
        if (service is null) return;

        var role = await _roleRepository.FindAsync(_generatedSuperAdminRoleId);
        if (role is null) return;

        var scope = await Scope.CreateKunderaScopeAsync(_kunderaScopeSecret, _scopeRepository);
        scope.AddService(service.Id);
        scope.AddRole(role.Id);
        await _scopeRepository.AddAsync(scope);
    }

    private async Task SeedCredentialAsync()
    {
        var user = await _userRepository.FindAsync(_adminUsername);
        if (user is null) return;
        await _credentialService.CreateAsync(UniqueIdentifier.From(_adminUsername), _adminPassword, user.Id.Value, IPAddress.None);
    }
}