using System.Net;
using Auth.Core.Domains;
using Auth.Core.Services;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Roles.Exceptions;
using Managements.Domain.Scopes;
using Managements.Domain.Services;
using Managements.Domain.Services.Exceptions;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Exception;
using Managements.Domain.Users;
using Microsoft.Extensions.Configuration;

namespace Managements.Data.Redis;

public class DefaultDataSeeder
{
    private const string RoleName = "superadmin";
    private const string Username = "superadmin";
    private const string UserGroupName = "administrator";
    private const string ScopeName = "global";
    private const string ServiceName = "all";
    private readonly string _adminPassword;

    private readonly IRoleRepository _roleRepository;
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IScopeRepository _scopeRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly ICredentialService _credentialService;


    public DefaultDataSeeder(IConfiguration configuration,
        IRoleRepository roleRepository,
        IUserGroupRepository userGroupRepository,
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        IScopeRepository scopeRepository,
        IServiceRepository serviceRepository,
        ICredentialService credentialService)
    {
        _roleRepository = roleRepository;
        _userGroupRepository = userGroupRepository;
        _userRepository = userRepository;
        _permissionRepository = permissionRepository;
        _scopeRepository = scopeRepository;
        _serviceRepository = serviceRepository;
        _credentialService = credentialService;
        _adminPassword = configuration.GetSection("AdminPassword")
            .Value;
    }


    public async Task SeedAsync()
    {
        await SeedRoleAsync();
        await SeedUserGroupAsync();
        await SeedUserAsync();
        await SeedServiceAsync();
        await SeedScopeAsync();
        await SeedCredentialAsync();
    }

    private async Task SeedRoleAsync()
    {
        var role = await _roleRepository.FindAsync(RoleId.From(RoleName));

        if (role is not null) return;

        role = await Role.FromAsync(RoleName, _roleRepository);

        var permissions = await _permissionRepository.FindAllAsync();
        foreach (var permission in permissions)
        {
            role.AddPermission(permission.Id);
        }

        await _roleRepository.AddAsync(role);
    }


    private async Task SeedUserGroupAsync()
    {
        var role = await _roleRepository.FindAsync(RoleId.From(RoleName));
        if (role is null)
        {
            throw new RoleNotSeededException();
        }

        var userGroup = await _userGroupRepository.FindAsync(UserGroupName);

        if (userGroup is not null) return;

        userGroup = await UserGroup.FromAsync(UserGroupName, role.Id, _userGroupRepository);
        await _userGroupRepository.AddAsync(userGroup);
    }

    private async Task SeedUserAsync()
    {
        var userGroup = await _userGroupRepository.FindAsync(UserGroupName);
        if (userGroup is null)
        {
            throw new UserGroupNotSeededException();
        }

        var exists = await _userRepository.ExistsAsync(Username);

        if (exists) return;

        var user = await User.CreateAsync(Username, userGroup.Id, _userRepository);
        await _userRepository.AddAsync(user);
    }

    private async Task SeedServiceAsync()
    {
        var service = await _serviceRepository.FindAsync(ServiceId.From(ServiceName));

        if (service is not null) return;

        service = await Service.FromAsync(ServiceName, _serviceRepository);
        await _serviceRepository.AddAsync(service);
    }

    private async Task SeedScopeAsync()
    {
        var scope = await _scopeRepository.FindAsync(ScopeId.From(ScopeName));

        if (scope is not null) return;

        scope = await Scope.FromAsync(ScopeName, _scopeRepository);
        var service = await _serviceRepository.FindAsync(ServiceId.From(ServiceName));
        if (service is null)
        {
            throw new ServiceNotSeededException();
        }

        scope.AddService(service.Id);

        var role = await _roleRepository.FindAsync(RoleId.From(RoleName));
        if (role is null)
        {
            throw new RoleNotSeededException();
        }

        scope.AddRole(role.Id);
        await _scopeRepository.AddAsync(scope);
    }

    private async Task SeedCredentialAsync()
    {
        var user = await _userRepository.FindAsync(Username);
        await _credentialService.CreateAsync(UniqueIdentifier.From(Username), _adminPassword, user.Id.Value, IPAddress.None);
    }
}