using AutoMapper;
using RoleManagements.Domain.Permissions;

namespace RoleManagement.Data.Redis.Permissions;

public class PermissionMappingProfile : Profile
{
    public PermissionMappingProfile()
    {
        CreateMap<Permission, PermissionDataModel>()
            .ForMember(dest => dest.Meta, 
                opt => opt.MapFrom("_meta"));
    }
}