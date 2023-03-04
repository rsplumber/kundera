using AutoMapper;
using Core.Domains.Auth;
using Core.Domains.Auth.Credentials;

namespace Data.Auth.Credentials;

internal sealed class CredentialMappingProfile : Profile
{
    public CredentialMappingProfile()
    {
        CreateMap<Password, PasswordType>().ConvertUsing(password => new PasswordType
        {
            Salt = password.Salt,
            Value = password.Value
        });
        CreateMap<PasswordType, Password>().ConvertUsing(passwordType => Password.From(passwordType.Value, passwordType.Salt));

        CreateMap<AuthActivity, CredentialActivityDataModel>().ConvertUsing(credentialActivity => new CredentialActivityDataModel
        {
            Id = credentialActivity.Id,
            CreatedDateUtc = credentialActivity.CreatedDateUtc,
            Agent = credentialActivity.Agent,
            IpAddress = credentialActivity.IpAddress
        });

        CreateMap<CredentialActivityDataModel, AuthActivity>().ConvertUsing(credentialActivityDataModel => new AuthActivity()
        {
            Id = credentialActivityDataModel.Id,
            IpAddress = credentialActivityDataModel.IpAddress,
            Agent = credentialActivityDataModel.Agent,
            CreatedDateUtc = credentialActivityDataModel.CreatedDateUtc,
        });
        DisableConstructorMapping();
        CreateMap<CredentialDataModel, Credential>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(credential => credential.Username, expression => expression.MapFrom(model => model.Username))
            .ForMember(credential => credential.User, expression => expression.MapFrom(model => model.UserId))
            .ForMember(credential => credential.Password, expression => expression.MapFrom(model => model.Password))
            .ForMember(credential => credential.FirstActivity, expression => expression.MapFrom(model => model.FirstActivity))
            .ForMember(credential => credential.LastActivity, expression => expression.MapFrom(model => model.LastActivity))
            .ForMember(credential => credential.ExpiresAtUtc, expression => expression.MapFrom(model => model.ExpiresAtUtc == null ? model.ExpiresAtUtc : model.ExpiresAtUtc.Value.ToUniversalTime()))
            .ForMember(credential => credential.CreatedDateUtc, expression => expression.MapFrom(model => model.CreatedDateUtc.ToUniversalTime()))
            .ForMember(credential => credential.OneTime, expression => expression.MapFrom(model => model.OneTime))
            .ForMember(credential => credential.SingleSession, expression => expression.MapFrom(model => model.SingleSession))
            .ForMember(credential => credential.SessionExpireTimeInMinutes, expression => expression.MapFrom(model => model.SessionExpireTimeInMinutes))
            .ReverseMap();
    }
}