using Core.Domains;
using Core.Domains.Auth.Credentials;
using Core.Domains.Groups;
using Core.Domains.Permissions;
using Core.Domains.Roles;
using Core.Domains.Scopes;
using Core.Domains.Services;
using Core.Domains.Users;
using Microsoft.Extensions.Configuration;

namespace Application.Seeders;

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
            scope.Add(service);
            scope.Add(role);
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
            await _credentialFactory.CreateAsync(_adminUsername, _adminPassword, user.Id, 1000);
        }

        await SeedPermissions(service);
    }

    private async Task SeedPermissions(Service service)
    {
        if (service.Permissions.Any()) return;
        SeedPermissionPermissions();
        SeedRolePermissions();
        SeedServicePermissions();
        SeedScopePermissions();
        SeedGroupPermissions();
        SeedUserPermissions();
        SeedCredentialsPermissions();
        SeedSessionsPermissions();

        var role = await _roleRepository.FindByNameAsync(EntityBaseValues.SuperAdminRole);
        if (role is not null)
        {
            foreach (var permission in service.Permissions)
            {
                role.Add(permission);
            }

            await _roleRepository.UpdateAsync(role);
        }

        await _serviceRepository.UpdateAsync(service);

        void SeedPermissionPermissions()
        {
            service.AddPermission("permissions_create");
            service.AddPermission("permissions_list");
            service.AddPermission("permissions_get");
            service.AddPermission("permissions_delete");
            service.AddPermission("permissions_change_meta");
        }

        void SeedRolePermissions()
        {
            service.AddPermission("roles_create");
            service.AddPermission("roles_list");
            service.AddPermission("roles_get");
            service.AddPermission("roles_delete");
            service.AddPermission("roles_add_permission");
            service.AddPermission("roles_remove_permission");
            service.AddPermission("roles_change_meta");
            service.AddPermission("roles_permissions_list");
        }

        void SeedServicePermissions()
        {
            service.AddPermission("services_create");
            service.AddPermission("services_list");
            service.AddPermission("services_get");
            service.AddPermission("services_delete");
            service.AddPermission("services_activate");
            service.AddPermission("services_de-activate");
        }

        void SeedScopePermissions()
        {
            service.AddPermission("scopes_create");
            service.AddPermission("scopes_list");
            service.AddPermission("scopes_get");
            service.AddPermission("scopes_delete");
            service.AddPermission("scopes_activate");
            service.AddPermission("scopes_de-activate");
            service.AddPermission("scopes_add_service");
            service.AddPermission("scopes_remove_service");
            service.AddPermission("scopes_add_role");
            service.AddPermission("scopes_remove_role");
            service.AddPermission("scopes_roles_list");
            service.AddPermission("scopes_sessions_list");
        }

        void SeedGroupPermissions()
        {
            service.AddPermission("groups_create");
            service.AddPermission("groups_list");
            service.AddPermission("groups_get");
            service.AddPermission("groups_delete");
            service.AddPermission("groups_assign_role");
            service.AddPermission("groups_revoke_role");
            service.AddPermission("groups_set_parent");
            service.AddPermission("groups_move_parent");
            service.AddPermission("groups_remove_parent");
            service.AddPermission("groups_enable");
            service.AddPermission("groups_disable");
        }

        void SeedUserPermissions()
        {
            service.AddPermission("users_create");
            service.AddPermission("users_list");
            service.AddPermission("users_get");
            service.AddPermission("users_delete");
            service.AddPermission("users_add_username");
            service.AddPermission("users_remove_username");
            service.AddPermission("users_exist_username");
            service.AddPermission("users_assign_role");
            service.AddPermission("users_revoke_role");
            service.AddPermission("users_join_group");
            service.AddPermission("users_remove_group");
            service.AddPermission("users_activate");
            service.AddPermission("users_suspend");
            service.AddPermission("users_block");
            service.AddPermission("users_sessions_list");
        }

        void SeedCredentialsPermissions()
        {
            service.AddPermission("credentials_create");
            service.AddPermission("credentials_create_onetime");
            service.AddPermission("credentials_create_time-periodic");
            service.AddPermission("credentials_delete");
        }

        void SeedSessionsPermissions()
        {
            service.AddPermission("sessions_terminate");
        }
    }
}