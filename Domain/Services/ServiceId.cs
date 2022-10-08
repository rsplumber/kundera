using Tes.Domain.Contracts;

namespace Domain.Services;

public class ServiceId : CustomType<string, ServiceId>, IIdentity
{
}