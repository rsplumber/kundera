using Core.Domains;
using Core.Domains.Auth.Credentials;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using Microsoft.Extensions.Configuration;

namespace Seeders;

internal sealed class DataSeeder
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
        var role = await _roleRepository.FindByNameAsync(EntityBaseValues.SuperAdminRole);
        if (role is null)
        {
            role = await _roleFactory.CreateAsync(EntityBaseValues.SuperAdminRole);
            await _roleRepository.UpdateAsync(role);
        }

        var group = await _groupRepository.FindByNameAsync(EntityBaseValues.AdministratorGroup);
        if (group is null)
        {
            group = await _groupFactory.CreateAdministratorAsync();
        }

        var service = await _serviceRepository.FindByNameAsync(EntityBaseValues.KunderaServiceName);
        if (service is null)
        {
            service = await _serviceFactory.CreateKunderaServiceAsync(_kunderaServiceSecret);
        }

        var scope = await _scopeRepository.FindByNameAsync(EntityBaseValues.IdentityScopeName);
        if (scope is null)
        {
            scope = await _scopeFactory.CreateIdentityScopeAsync(_identityScopeSecret);
            scope.AddService(service.Id);
            scope.AddRole(role.Id);
            await _scopeRepository.UpdateAsync(scope);
        }

        var user = await _userRepository.FindByUsernameAsync(_adminUsername);
        if (user is null)
        {
            user = await _userFactory.CreateAsync(_adminUsername, group.Id);
        }

        var credentials = await _credentialRepository.FindByUsernameAsync(_adminUsername);
        if (!credentials.Any(credential => credential.Password.Check(_adminPassword)))
        {
            await _credentialFactory.CreateAsync(_adminUsername, _adminPassword, user.Id);
        }

        await SeedPermissions();

        await SeedServiceManAsync();
    }

    private async Task SeedPermissions()
    {
        var firstPermission = await _permissionRepository.FindByNameAsync("kundera_permissions_create");
        if (firstPermission is not null) return;

        await SeedPermissionPermissions();
        await SeedRolePermissions();
        await SeedServicePermissions();
        await SeedScopePermissions();
        await SeedGroupPermissions();
        await SeedUserPermissions();
        await SeedCredentialsPermissions();
        await SeedSessionsPermissions();

        var role = await _roleRepository.FindByNameAsync(EntityBaseValues.SuperAdminRole);
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
        await _permissionFactory.CreateAsync("kundera_permissions_create");
        await _permissionFactory.CreateAsync("kundera_permissions_list");
        await _permissionFactory.CreateAsync("kundera_permissions_get");
        await _permissionFactory.CreateAsync("kundera_permissions_delete");
        await _permissionFactory.CreateAsync("kundera_permissions_change_meta");
    }

    private async Task SeedRolePermissions()
    {
        await _permissionFactory.CreateAsync("kundera_roles_create");
        await _permissionFactory.CreateAsync("kundera_roles_list");
        await _permissionFactory.CreateAsync("kundera_roles_get");
        await _permissionFactory.CreateAsync("kundera_roles_delete");
        await _permissionFactory.CreateAsync("kundera_roles_add_permission");
        await _permissionFactory.CreateAsync("kundera_roles_remove_permission");
        await _permissionFactory.CreateAsync("kundera_roles_change_meta");
        await _permissionFactory.CreateAsync("kundera_roles_permissions_list");
    }

    private async Task SeedServicePermissions()
    {
        await _permissionFactory.CreateAsync("kundera_services_create");
        await _permissionFactory.CreateAsync("kundera_services_list");
        await _permissionFactory.CreateAsync("kundera_services_get");
        await _permissionFactory.CreateAsync("kundera_services_delete");
        await _permissionFactory.CreateAsync("kundera_services_activate");
        await _permissionFactory.CreateAsync("kundera_services_de-activate");
    }

    private async Task SeedScopePermissions()
    {
        await _permissionFactory.CreateAsync("kundera_scopes_create");
        await _permissionFactory.CreateAsync("kundera_scopes_list");
        await _permissionFactory.CreateAsync("kundera_scopes_get");
        await _permissionFactory.CreateAsync("kundera_scopes_delete");
        await _permissionFactory.CreateAsync("kundera_scopes_activate");
        await _permissionFactory.CreateAsync("kundera_scopes_de-activate");
        await _permissionFactory.CreateAsync("kundera_scopes_add_service");
        await _permissionFactory.CreateAsync("kundera_scopes_remove_service");
        await _permissionFactory.CreateAsync("kundera_scopes_add_role");
        await _permissionFactory.CreateAsync("kundera_scopes_remove_role");
        await _permissionFactory.CreateAsync("kundera_scopes_roles_list");
        await _permissionFactory.CreateAsync("kundera_scopes_sessions_list");
    }

    private async Task SeedGroupPermissions()
    {
        await _permissionFactory.CreateAsync("kundera_groups_create");
        await _permissionFactory.CreateAsync("kundera_groups_list");
        await _permissionFactory.CreateAsync("kundera_groups_get");
        await _permissionFactory.CreateAsync("kundera_groups_delete");
        await _permissionFactory.CreateAsync("kundera_groups_assign_role");
        await _permissionFactory.CreateAsync("kundera_groups_revoke_role");
        await _permissionFactory.CreateAsync("kundera_groups_set_parent");
        await _permissionFactory.CreateAsync("kundera_groups_move_parent");
        await _permissionFactory.CreateAsync("kundera_groups_remove_parent");
        await _permissionFactory.CreateAsync("kundera_groups_enable");
        await _permissionFactory.CreateAsync("kundera_groups_disable");
    }

    private async Task SeedUserPermissions()
    {
        await _permissionFactory.CreateAsync("kundera_users_create");
        await _permissionFactory.CreateAsync("kundera_users_list");
        await _permissionFactory.CreateAsync("kundera_users_get");
        await _permissionFactory.CreateAsync("kundera_users_delete");
        await _permissionFactory.CreateAsync("kundera_users_add_username");
        await _permissionFactory.CreateAsync("kundera_users_remove_username");
        await _permissionFactory.CreateAsync("kundera_users_exist_username");
        await _permissionFactory.CreateAsync("kundera_users_assign_role");
        await _permissionFactory.CreateAsync("kundera_users_revoke_role");
        await _permissionFactory.CreateAsync("kundera_users_join_group");
        await _permissionFactory.CreateAsync("kundera_users_remove_group");
        await _permissionFactory.CreateAsync("kundera_users_activate");
        await _permissionFactory.CreateAsync("kundera_users_suspend");
        await _permissionFactory.CreateAsync("kundera_users_block");
        await _permissionFactory.CreateAsync("kundera_users_sessions_list");
    }

    private async Task SeedCredentialsPermissions()
    {
        await _permissionFactory.CreateAsync("kundera_credentials_create");
        await _permissionFactory.CreateAsync("kundera_credentials_create_onetime");
        await _permissionFactory.CreateAsync("kundera_credentials_create_time-periodic");
        await _permissionFactory.CreateAsync("kundera_credentials_delete");
    }

    private async Task SeedSessionsPermissions()
    {
        await _permissionFactory.CreateAsync("kundera_sessions_terminate");
    }


    private async Task SeedServiceManAsync()
    {
        var serviceAdminRole = await _roleRepository.FindByNameAsync(EntityBaseValues.ServiceAdminRole);
        if (serviceAdminRole is null)
        {
            serviceAdminRole = await _roleFactory.CreateAsync(EntityBaseValues.ServiceAdminRole);
        }

        var permissions = await Task.WhenAll(
            _permissionRepository.FindByNameAsync("kundera_scopes_list"),
            _permissionRepository.FindByNameAsync("kundera_scopes_get"),
            _permissionRepository.FindByNameAsync("kundera_scopes_roles_list"),
            _permissionRepository.FindByNameAsync("kundera_roles_create"),
            _permissionRepository.FindByNameAsync("kundera_roles_get"),
            _permissionRepository.FindByNameAsync("kundera_roles_delete"),
            _permissionRepository.FindByNameAsync("kundera_roles_add_permission"),
            _permissionRepository.FindByNameAsync("kundera_roles_remove_permission"),
            _permissionRepository.FindByNameAsync("kundera_roles_change_meta"),
            _permissionRepository.FindByNameAsync("kundera_roles_permissions_list"),
            _permissionRepository.FindByNameAsync("kundera_permissions_create"),
            _permissionRepository.FindByNameAsync("kundera_permissions_list"),
            _permissionRepository.FindByNameAsync("kundera_permissions_get"),
            _permissionRepository.FindByNameAsync("kundera_permissions_delete"),
            _permissionRepository.FindByNameAsync("kundera_permissions_change_meta"),
            _permissionRepository.FindByNameAsync("kundera_services_get"),
            _permissionRepository.FindByNameAsync("kundera_services_activate"),
            _permissionRepository.FindByNameAsync("kundera_services_de-activate")
        );

        if (permissions.Any(permission => permission is null))
        {
            throw new ApplicationException("Cannot create service admin role");
        }


        foreach (var permission in permissions)
        {
            serviceAdminRole.AddPermission(permission!.Id);
        }

        await _roleRepository.UpdateAsync(serviceAdminRole);

        var group = await _groupRepository.FindByNameAsync(EntityBaseValues.ServiceManGroup);
        if (group is null)
        {
            await _groupFactory.CreateAsync(EntityBaseValues.ServiceManGroup, serviceAdminRole.Id);
        }
    }
}