using Tes.Domain.Contracts;

namespace RoleManagements.Domain.Services.Types;

public class ServiceId : CustomType<string, ServiceId>, IIdentity
{
}