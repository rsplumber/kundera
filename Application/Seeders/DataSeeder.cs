using Core;
using Core.Auth.Credentials;
using Core.Groups;
using Core.Permissions;
using Core.Roles;
using Core.Scopes;
using Core.Services;
using Core.Users;

namespace Application.Seeders;

internal sealed class DataSeeder
{
    private readonly string _adminUsername;
    private readonly string _adminPassword;
    private readonly string _kunderaServiceSecret;

    private readonly IUserFactory _userFactory;
    private readonly IServiceFactory _serviceFactory;
    private readonly IScopeFactory _scopeFactory;
    private readonly IRoleFactory _roleFactory;
    private readonly IGroupFactory _groupFactory;

    private readonly IRoleRepository _roleRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPermissionService _permissionService;
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICredentialFactory _credentialFactory;
    private readonly ICredentialRepository _credentialRepository;

    public DataSeeder(IConfiguration configuration,
        IRoleRepository roleRepository,
        IGroupRepository groupRepository,
        IUserRepository userRepository,
        IScopeRepository scopeRepository,
        IServiceRepository serviceRepository,
        IUserFactory userFactory,
        IServiceFactory serviceFactory,
        IScopeFactory scopeFactory,
        IRoleFactory roleFactory,
        IGroupFactory groupFactory,
        ICredentialFactory credentialFactory,
        ICredentialRepository credentialRepository,
        IPermissionService permissionService)
    {
        _roleRepository = roleRepository;
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
        _userFactory = userFactory;
        _serviceFactory = serviceFactory;
        _scopeFactory = scopeFactory;
        _roleFactory = roleFactory;
        _groupFactory = groupFactory;
        _credentialFactory = credentialFactory;
        _credentialRepository = credentialRepository;
        _permissionService = permissionService;
        _adminUsername = configuration.GetSection("DefaultConfigs:AdminUsername").Value ?? throw new ArgumentNullException(nameof(_adminUsername));
        _adminPassword = configuration.GetSection("DefaultConfigs:AdminPassword").Value ?? throw new ArgumentNullException(nameof(_adminPassword));
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
            scope = await _scopeFactory.CreateIdentityScopeAsync();
            scope.Add(service);
            scope.Add(role);
            await _scopeRepository.UpdateAsync(scope);
        }

        var credentials = await _credentialRepository.FindByUsernameAsync(_adminUsername);
        if (!credentials.Any(credential => credential.Password.Check(_adminPassword)))
        {
            var user = await _userFactory.CreateAsync(group.Id);
            await _credentialFactory.CreateAsync(user.Id, _adminUsername, _adminPassword,
                1000,
                1000,
                true);
        }

        await SeedPermissions(service);
    }

    private async Task SeedPermissions(Service service)
    {
        if (service.Permissions.Any()) return;
        await SeedPermissionPermissionsAsync();
        await SeedRolePermissionsAsync();
        await SeedServicePermissionsAsync();
        await SeedScopePermissionsAsync();
        await SeedGroupPermissionsAsync();
        await SeedUserPermissionsAsync();
        await SeedCredentialsPermissionsAsync();
        await SeedSessionsPermissionsAsync();

        var updatedService = await _serviceRepository.FindAsync(service.Id);
        if (updatedService is null) return;
        var role = await _roleRepository.FindByNameAsync(EntityBaseValues.SuperAdminRole);
        if (role is not null)
        {
            foreach (var permission in updatedService.Permissions)
            {
                role.Add(permission);
            }

            await _roleRepository.UpdateAsync(role);
        }

        async Task SeedPermissionPermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "permissions_create");
            await _permissionService.CreateAsync(service, "permissions_list");
            await _permissionService.CreateAsync(service, "permissions_get");
            await _permissionService.CreateAsync(service, "permissions_delete");
            await _permissionService.CreateAsync(service, "permissions_change_meta");
        }

        async Task SeedRolePermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "roles_create");
            await _permissionService.CreateAsync(service, "roles_list");
            await _permissionService.CreateAsync(service, "roles_get");
            await _permissionService.CreateAsync(service, "roles_delete");
            await _permissionService.CreateAsync(service, "roles_add_permission");
            await _permissionService.CreateAsync(service, "roles_remove_permission");
            await _permissionService.CreateAsync(service, "roles_change_meta");
            await _permissionService.CreateAsync(service, "roles_permissions_list");
        }

        async Task SeedServicePermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "services_create");
            await _permissionService.CreateAsync(service, "services_list");
            await _permissionService.CreateAsync(service, "services_get");
            await _permissionService.CreateAsync(service, "services_delete");
            await _permissionService.CreateAsync(service, "services_activate");
            await _permissionService.CreateAsync(service, "services_de-activate");
        }

        async Task SeedScopePermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "scopes_create");
            await _permissionService.CreateAsync(service, "scopes_list");
            await _permissionService.CreateAsync(service, "scopes_get");
            await _permissionService.CreateAsync(service, "scopes_delete");
            await _permissionService.CreateAsync(service, "scopes_activate");
            await _permissionService.CreateAsync(service, "scopes_de-activate");
            await _permissionService.CreateAsync(service, "scopes_add_service");
            await _permissionService.CreateAsync(service, "scopes_remove_service");
            await _permissionService.CreateAsync(service, "scopes_add_role");
            await _permissionService.CreateAsync(service, "scopes_remove_role");
            await _permissionService.CreateAsync(service, "scopes_roles_list");
            await _permissionService.CreateAsync(service, "scopes_sessions_list");
        }

        async Task SeedGroupPermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "groups_create");
            await _permissionService.CreateAsync(service, "groups_list");
            await _permissionService.CreateAsync(service, "groups_get");
            await _permissionService.CreateAsync(service, "groups_delete");
            await _permissionService.CreateAsync(service, "groups_assign_role");
            await _permissionService.CreateAsync(service, "groups_revoke_role");
            await _permissionService.CreateAsync(service, "groups_set_parent");
            await _permissionService.CreateAsync(service, "groups_move_parent");
            await _permissionService.CreateAsync(service, "groups_remove_parent");
            await _permissionService.CreateAsync(service, "groups_enable");
            await _permissionService.CreateAsync(service, "groups_disable");
        }

        async Task SeedUserPermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "users_create");
            await _permissionService.CreateAsync(service, "users_list");
            await _permissionService.CreateAsync(service, "users_get");
            await _permissionService.CreateAsync(service, "users_delete");
            await _permissionService.CreateAsync(service, "users_add_username");
            await _permissionService.CreateAsync(service, "users_username_credential");
            await _permissionService.CreateAsync(service, "users_remove_username");
            await _permissionService.CreateAsync(service, "users_exist_username");
            await _permissionService.CreateAsync(service, "users_assign_role");
            await _permissionService.CreateAsync(service, "users_revoke_role");
            await _permissionService.CreateAsync(service, "users_join_group");
            await _permissionService.CreateAsync(service, "users_remove_group");
            await _permissionService.CreateAsync(service, "users_activate");
            await _permissionService.CreateAsync(service, "users_suspend");
            await _permissionService.CreateAsync(service, "users_block");
            await _permissionService.CreateAsync(service, "users_sessions_list");
        }

        async Task SeedCredentialsPermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "credentials_create");
            await _permissionService.CreateAsync(service, "credentials_create_onetime");
            await _permissionService.CreateAsync(service, "credentials_create_time-periodic");
            await _permissionService.CreateAsync(service, "credentials_delete");
            await _permissionService.CreateAsync(service, "credentials_session_terminate");
            await _permissionService.CreateAsync(service, "credentials_password_change");
        }

        async Task SeedSessionsPermissionsAsync()
        {
            await _permissionService.CreateAsync(service, "sessions_terminate");
        }
    }
}