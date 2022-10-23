using System.Net;
using Auth.Core;
using Auth.Core.Services;
using Kite.Hashing;
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
    private const string RoleName = "superadmin";
    private const string UserGroupName = "administrator";
    private const string ScopeName = "identity";
    private const string ServiceName = "kundera";
    private readonly string _adminUsername;
    private readonly string _adminPassword;
    private RoleId _generatedRoleId;
    private ServiceId _generatedServiceId;

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
    }


    public async Task SeedAsync()
    {
        await SeedPermissions();
        await SeedRoleAsync();
        await SeedUserGroupAsync();
        await SeedUserAsync();
        await SeedServiceAsync();
        await SeedScopeAsync();
        await SeedCredentialAsync();
    }

    private async Task SeedPermissions()
    {
        await _permissionRepository.AddAsync(await Permission.FromAsync("", _permissionRepository));
    }

    private async Task SeedRoleAsync()
    {
        var exists = await _roleRepository.ExistsAsync(RoleName);

        if (exists) return;

        var role = await Role.FromAsync(RoleName, _roleRepository);

        var permissions = await _permissionRepository.FindAsync();
        foreach (var permission in permissions)
        {
            role.AddPermission(permission.Id);
        }

        await _roleRepository.AddAsync(role);

        _generatedRoleId = role.Id;
    }


    private async Task SeedUserGroupAsync()
    {
        var role = await _roleRepository.FindAsync(_generatedRoleId);
        if (role is null) return;

        var userGroup = await _userGroupRepository.FindAsync(UserGroupName);

        if (userGroup is not null) return;

        userGroup = await UserGroup.FromAsync(UserGroupName, role.Id, _userGroupRepository);
        await _userGroupRepository.AddAsync(userGroup);
    }

    private async Task SeedUserAsync()
    {
        var userGroup = await _userGroupRepository.FindAsync(UserGroupName);
        if (userGroup is null) return;

        var exists = await _userRepository.ExistsAsync(_adminUsername);

        if (exists) return;

        var user = await User.CreateAsync(_adminUsername, userGroup.Id, _userRepository);
        await _userRepository.AddAsync(user);
    }

    private async Task SeedServiceAsync()
    {
        var exists = await _serviceRepository.ExistsAsync(ServiceName);

        if (exists) return;

        var service = await Service.FromAsync(ServiceName, _hashService, _serviceRepository);
        await _serviceRepository.AddAsync(service);
        _generatedServiceId = service.Id;
    }

    private async Task SeedScopeAsync()
    {
        var exists = await _scopeRepository.ExistsAsync(ScopeName);

        if (exists) return;

        var scope = await Scope.FromAsync(ScopeName, _hashService, _scopeRepository);
        var service = await _serviceRepository.FindAsync(_generatedServiceId);
        if (service is null) return;

        scope.AddService(service.Id);

        var role = await _roleRepository.FindAsync(_generatedRoleId);
        if (role is null) return;

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