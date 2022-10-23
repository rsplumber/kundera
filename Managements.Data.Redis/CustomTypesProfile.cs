using AutoMapper;
using Managements.Domain;
using Managements.Domain.Permissions;
using Managements.Domain.Roles;
using Managements.Domain.Scopes;
using Managements.Domain.Scopes.Types;
using Managements.Domain.Services;
using Managements.Domain.Services.Types;
using Managements.Domain.UserGroups;
using Managements.Domain.UserGroups.Types;
using Managements.Domain.Users;
using Managements.Domain.Users.Types;

namespace Managements.Data;

internal sealed class CustomTypesProfile : Profile
{
    public CustomTypesProfile()
    {
        DisableConstructorMapping();
        CreateMap<Guid, UserId>()
            .ConvertUsing(u => UserId.From(u));

        CreateMap<UserId, Guid>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, Username>()
            .ConvertUsing(u => Username.From(u));

        CreateMap<Username, string>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, Name>()
            .ConvertUsing(u => Name.From(u));

        CreateMap<Name, string>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, UserStatus>()
            .ConvertUsing(u => UserStatus.From(u));

        CreateMap<UserStatus, string>()
            .ConvertUsing(u => u.Value);

        CreateMap<Guid, UserGroupId>()
            .ConvertUsing(u => UserGroupId.From(u));

        CreateMap<UserGroupId, Guid>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, UserGroupStatus>()
            .ConvertUsing(u => UserGroupStatus.From(u));

        CreateMap<UserGroupStatus, string>()
            .ConvertUsing(u => u.Value);

        CreateMap<Guid, ServiceId>()
            .ConvertUsing(s => ServiceId.From(s));

        CreateMap<ServiceId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ServiceStatus>()
            .ConvertUsing(s => ServiceStatus.From(s));

        CreateMap<ServiceStatus, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ServiceSecret>()
            .ConvertUsing(s => ServiceSecret.From(s));

        CreateMap<ServiceSecret, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<Guid, ScopeId>()
            .ConvertUsing(s => ScopeId.From(s));

        CreateMap<ScopeId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ScopeStatus>()
            .ConvertUsing(s => ScopeStatus.From(s));

        CreateMap<ScopeStatus, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ScopeSecret>()
            .ConvertUsing(s => ScopeSecret.From(s));

        CreateMap<ScopeSecret, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<Guid, RoleId>()
            .ConvertUsing(s => RoleId.From(s));

        CreateMap<RoleId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<Guid, PermissionId>()
            .ConvertUsing(s => PermissionId.From(s));

        CreateMap<PermissionId, Guid>()
            .ConvertUsing(s => s.Value);
    }
}