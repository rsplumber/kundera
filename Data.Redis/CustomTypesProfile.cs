using System.Net;
using AutoMapper;
using Core.Domains;
using Core.Domains.Auth.Credentials;
using Core.Domains.Auth.Sessions;
using Core.Domains.Groups.Types;
using Core.Domains.Permissions.Types;
using Core.Domains.Roles.Types;
using Core.Domains.Scopes.Types;
using Core.Domains.Services.Types;
using Core.Domains.Users;
using Core.Domains.Users.Types;
using Managements.Data.Auth.Credentials;

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
            .ConvertUsing(u => Enumeration.GetAll<UserStatus>().First(status => status.Name == u));

        CreateMap<UserStatus, string>()
            .ConvertUsing(u => u.Name);

        CreateMap<Guid, GroupId>()
            .ConvertUsing(u => GroupId.From(u));

        CreateMap<GroupId, Guid>()
            .ConvertUsing(u => u.Value);

        CreateMap<string, GroupStatus>()
            .ConvertUsing(u => Enumeration.GetAll<GroupStatus>().First(status => status.Name == u));

        CreateMap<GroupStatus, string>()
            .ConvertUsing(u => u.Name);

        CreateMap<Guid, ServiceId>()
            .ConvertUsing(s => ServiceId.From(s));

        CreateMap<ServiceId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ServiceStatus>()
            .ConvertUsing(s => Enumeration.GetAll<ServiceStatus>().First(status => status.Name == s));

        CreateMap<ServiceStatus, string>()
            .ConvertUsing(s => s.Name);

        CreateMap<string, ServiceSecret>()
            .ConvertUsing(s => ServiceSecret.From(s));

        CreateMap<ServiceSecret, string>()
            .ConvertUsing(s => s.Value);

        CreateMap<Guid, ScopeId>()
            .ConvertUsing(s => ScopeId.From(s));

        CreateMap<ScopeId, Guid>()
            .ConvertUsing(s => s.Value);

        CreateMap<string, ScopeStatus>()
            .ConvertUsing(s => Enumeration.GetAll<ScopeStatus>().First(status => status.Name == s));

        CreateMap<ScopeStatus, string>()
            .ConvertUsing(s => s.Name);

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

        CreateMap<string, Token>().ConvertUsing(s => Token.From(s));
        CreateMap<Token, string>().ConvertUsing(token => token.Value);

        CreateMap<string, UniqueIdentifier>().ConvertUsing(id => UniqueIdentifier.Parse(id));
        CreateMap<UniqueIdentifier, string>().ConvertUsing(identifier => identifier.Value);

        CreateMap<Password, PasswordType>().ConvertUsing(password => new PasswordType
        {
            Salt = password.Salt,
            Value = password.Value
        });
        CreateMap<PasswordType, Password>().ConvertUsing(passwordType => Password.From(passwordType.Value, passwordType.Salt));

        CreateMap<string, IPAddress>().ConvertUsing(s => IPAddress.Parse(s));
        CreateMap<IPAddress, string>().ConvertUsing(ip => ip.ToString());
    }
}