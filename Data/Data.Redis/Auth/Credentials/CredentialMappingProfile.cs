using AutoMapper;
using Core.Auth.Credentials;
using Core.Auth.Sessions;

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

        CreateMap<AuthorizationActivity, AuthenticationActivityDataModel>().ConvertUsing(credentialActivity => new AuthenticationActivityDataModel
        {
            Id = credentialActivity.Id,
            CreatedDateUtc = credentialActivity.CreatedDateUtc,
            Agent = credentialActivity.Agent,
            IpAddress = credentialActivity.IpAddress
        });

        CreateMap<AuthenticationActivityDataModel, AuthenticationActivity>().ConvertUsing(credentialActivityDataModel => new AuthenticationActivity()
        {
            Id = credentialActivityDataModel.Id,
            Credential = credentialActivityDataModel.CredentialId,
            IpAddress = credentialActivityDataModel.IpAddress,
            Agent = credentialActivityDataModel.Agent,
            CreatedDateUtc = credentialActivityDataModel.CreatedDateUtc
        });
        DisableConstructorMapping();
        CreateMap<CredentialDataModel, Credential>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(credential => credential.Id, expression => expression.MapFrom(model => model.Id))
            .ForMember(credential => credential.Username, expression => expression.MapFrom(model => model.Username))
            .ForMember(credential => credential.User, expression => expression.MapFrom(model => model.UserId))
            .ForMember(credential => credential.Password, expression => expression.MapFrom(model => model.Password))
            .ForMember(credential => credential.CreatedDateUtc, expression => expression.MapFrom(model => model.CreatedDateUtc.ToUniversalTime()))
            .ForMember(credential => credential.OneTime, expression => expression.MapFrom(model => model.OneTime))
            .ForMember(credential => credential.SingleSession, expression => expression.MapFrom(model => model.SingleSession))
            .ReverseMap();
    }
}