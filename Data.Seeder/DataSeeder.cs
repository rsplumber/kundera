using System.Net;
using Core.Domains;
using Core.Domains.Credentials;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Scopes.Types;
using Core.Domains.Services;
using Core.Domains.Services.Types;
using Core.Domains.Users;
using Microsoft.Extensions.Configuration;

namespace Data.Seeder;

public class DataSeeder
{
    private readonly string _adminUsername;
    private readonly string _adminPassword;
    private readonly string _identityScopeSecret;
    private readonly string _kunderaServiceSecret;

    private readonly IUserFactory _userFactory;
    private readonly IServiceFactory _serviceFactory;
    private readonly IScopeFactory _scopeFactory;
    private readonly IRoleFactory _roleFactory;
    private readonly IPermissionFactory _permissionFactory;
    private readonly IGroupFactory _groupFactory;

    private readonly IRoleRepository _roleRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICredentialFactory _credentialFactory;
    private readonly ICredentialRepository _credentialRepository;

    public DataSeeder(IConfiguration configuration,
        IRoleRepository roleRepository,
        IGroupRepository groupRepository,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IScopeRepository scopeRepository,
        IServiceRepository serviceRepository,
        IUserFactory userFactory,
        IServiceFactory serviceFactory,
        IScopeFactory scopeFactory,
        IRoleFactory roleFactory,
        IPermissionFactory permissionFactory,
        IGroupFactory groupFactory,
        ICredentialFactory credentialFactory,
        ICredentialRepository credentialRepository)
    {
        _roleRepository = roleRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
        _userFactory = userFactory;
        _serviceFactory = serviceFactory;
        _scopeFactory = scopeFactory;
        _roleFactory = roleFactory;
        _permissionFactory = permissionFactory;
        _groupFactory = groupFactory;
        _credentialFactory = credentialFactory;
        _credentialRepository = credentialRepository;
        _adminUsername = configuration.GetSection("DefaultConfigs:AdminUsername").Value ?? throw new ArgumentNullException(nameof(_adminUsername));
        _adminPassword = configuration.GetSection("DefaultConfigs:AdminPassword").Value ?? throw new ArgumentNullException(nameof(_adminPassword));
        _identityScopeSecret = configuration.GetSection("DefaultConfigs:IdentityScopeSecret").Value ?? throw new ArgumentNullException(nameof(_identityScopeSecret));
        _kunderaServiceSecret = configuration.GetSection("DefaultConfigs:KunderaServiceSecret").Value ?? throw new ArgumentNullException(nameof(_kunderaServiceSecret));
    }


    public async Task SeedAsync()
    {
        var role = await _roleRepository.FindAsync(EntityBaseValues.SuperAdminRole);
        if (role is null)
        {
            role = await _roleFactory.CreateAsync(EntityBaseValues.SuperAdminRole);
            await _roleRepository.UpdateAsync(role);
        }

        var group = await _groupRepository.FindAsync(EntityBaseValues.AdministratorGroup);
        if (group is null)
        {
            group = await _groupFactory.CreateAdministratorAsync();
        }

        var service = await _serviceRepository.FindAsync(EntityBaseValues.KunderaServiceName);
        if (service is null)
        {
            service = await _serviceFactory.CreateKunderaServiceAsync(ServiceSecret.From(_kunderaServiceSecret));
        }

        var scope = await _scopeRepository.FindAsync(EntityBaseValues.IdentityScopeName);
        if (scope is null)
        {
            scope = await _scopeFactory.CreateIdentityScopeAsync(ScopeSecret.From(_identityScopeSecret));
            scope.AddService(service.Id);
            scope.AddRole(role.Id);
            await _scopeRepository.UpdateAsync(scope);
        }

        var user = await _userRepository.FindAsync(_adminUsername);
        if (user is null)
        {
            user = await _userFactory.CreateAsync(_adminUsername, group.Id);
        }

        var uniqueIdentifier = UniqueIdentifier.From(_adminUsername);
        var credential = await _credentialRepository.FindAsync(uniqueIdentifier);
        if (credential is null)
        {
            await _credentialFactory.CreateAsync(uniqueIdentifier, _adminPassword, user.Id, IPAddress.None);
        }

        await SeedPermissions();

        await SeedServiceManAsync();
    }

    private async Task SeedPermissions()
    {
        var exists = await _permissionRepository.ExistsAsync("permissions_create");
        if (exists) return;

        await SeedPermissionPermissions();
        await SeedRolePermissions();
        await SeedServicePermissions();
        await SeedScopePermissions();
        await SeedGroupPermissions();
        await SeedUserPermissions();

        var role = await _roleRepository.FindAsync(EntityBaseValues.SuperAdminRole);
        if (role is not null)
        {
            var permissions = await _permissionRepository.FindAsync();
            foreach (var permission in permissions)
            {
                role.AddPermission(permission.Id);
            }

            await _roleRepository.UpdateAsync(role);
        }
    }

    private async Task SeedPermissionPermissions()
    {
        await _permissionFactory.CreateAsync("permissions_create");
        await _permissionFactory.CreateAsync("permissions_list");
        await _permissionFactory.CreateAsync("permissions_get");
        await _permissionFactory.CreateAsync("permissions_delete");
        await _permissionFactory.CreateAsync("permissions_add_meta");
        await _permissionFactory.CreateAsync("permissions_remove_meta");
    }

    private async Task SeedRolePermissions()
    {
        await _permissionFactory.CreateAsync("roles_create");
        await _permissionFactory.CreateAsync("roles_list");
        await _permissionFactory.CreateAsync("roles_get");
        await _permissionFactory.CreateAsync("roles_delete");
        await _permissionFactory.CreateAsync("roles_add_permission");
        await _permissionFactory.CreateAsync("roles_remove_permission");
        await _permissionFactory.CreateAsync("roles_add_meta");
        await _permissionFactory.CreateAsync("roles_remove_meta");
        await _permissionFactory.CreateAsync("roles_permissions_list");
    }

    private async Task SeedServicePermissions()
    {
        await _permissionFactory.CreateAsync("services_create");
        await _permissionFactory.CreateAsync("services_list");
        await _permissionFactory.CreateAsync("services_get");
        await _permissionFactory.CreateAsync("services_delete");
        await _permissionFactory.CreateAsync("services_activate");
        await _permissionFactory.CreateAsync("services_de-activate");
    }

    private async Task SeedScopePermissions()
    {
        await _permissionFactory.CreateAsync("scopes_create");
        await _permissionFactory.CreateAsync("scopes_list");
        await _permissionFactory.CreateAsync("scopes_get");
        await _permissionFactory.CreateAsync("scopes_delete");
        await _permissionFactory.CreateAsync("scopes_active");
        await _permissionFactory.CreateAsync("scopes_de-active");
        await _permissionFactory.CreateAsync("scopes_add_service");
        await _permissionFactory.CreateAsync("scopes_remove_service");
        await _permissionFactory.CreateAsync("scopes_add_role");
        await _permissionFactory.CreateAsync("scopes_remove_role");
        await _permissionFactory.CreateAsync("scopes_roles_list");
    }

    private async Task SeedGroupPermissions()
    {
        await _permissionFactory.CreateAsync("groups_create");
        await _permissionFactory.CreateAsync("groups_list");
        await _permissionFactory.CreateAsync("groups_get");
        await _permissionFactory.CreateAsync("groups_delete");
        await _permissionFactory.CreateAsync("groups_assign_role");
        await _permissionFactory.CreateAsync("groups_revoke_role");
        await _permissionFactory.CreateAsync("groups_set_parent");
        await _permissionFactory.CreateAsync("groups_move_parent");
        await _permissionFactory.CreateAsync("groups_remove_parent");
        await _permissionFactory.CreateAsync("groups_enable");
        await _permissionFactory.CreateAsync("groups_disable");
    }

    private async Task SeedUserPermissions()
    {
        await _permissionFactory.CreateAsync("user_create");
        await _permissionFactory.CreateAsync("user_list");
        await _permissionFactory.CreateAsync("user_get");
        await _permissionFactory.CreateAsync("user_delete");
        await _permissionFactory.CreateAsync("user_add_username");
        await _permissionFactory.CreateAsync("user_remove_username");
        await _permissionFactory.CreateAsync("user_exist_username");
        await _permissionFactory.CreateAsync("user_assign_role");
        await _permissionFactory.CreateAsync("user_revoke_role");
        await _permissionFactory.CreateAsync("user_join_group");
        await _permissionFactory.CreateAsync("user_remove_group");
        await _permissionFactory.CreateAsync("user_activate");
        await _permissionFactory.CreateAsync("user_suspend");
        await _permissionFactory.CreateAsync("user_block");
    }


    private async Task SeedServiceManAsync()
    {
        var roleExists = await _roleRepository.ExistsAsync(EntityBaseValues.ServiceAdminRole);
        if (roleExists) return;

        var permissions = await Task.WhenAll(
            _permissionRepository.FindAsync("scopes_list"),
            _permissionRepository.FindAsync("scopes_get"),
            _permissionRepository.FindAsync("scopes_roles_list"),
            _permissionRepository.FindAsync("roles_create"),
            _permissionRepository.FindAsync("roles_get"),
            _permissionRepository.FindAsync("roles_delete"),
            _permissionRepository.FindAsync("roles_add_permission"),
            _permissionRepository.FindAsync("roles_remove_permission"),
            _permissionRepository.FindAsync("roles_add_meta"),
            _permissionRepository.FindAsync("roles_remove_meta"),
            _permissionRepository.FindAsync("roles_permissions_list"),
            _permissionRepository.FindAsync("permissions_create"),
            _permissionRepository.FindAsync("permissions_list"),
            _permissionRepository.FindAsync("permissions_get"),
            _permissionRepository.FindAsync("permissions_delete"),
            _permissionRepository.FindAsync("permissions_add_meta"),
            _permissionRepository.FindAsync("permissions_remove_meta"),
            _permissionRepository.FindAsync("services_get"),
            _permissionRepository.FindAsync("services_activate"),
            _permissionRepository.FindAsync("services_de-activate")
        );

        var role = await _roleFactory.CreateAsync(EntityBaseValues.ServiceAdminRole);
        foreach (var permission in permissions)
        {
            role.AddPermission(permission.Id);
        }

        await _roleRepository.UpdateAsync(role);

        var group = await _groupRepository.FindAsync(EntityBaseValues.ServiceManGroup);
        if (group is null)
        {
            await _groupFactory.CreateAsync(EntityBaseValues.ServiceManGroup, role.Id);
        }
    }
}