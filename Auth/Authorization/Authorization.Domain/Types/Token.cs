using Tes.Domain.Contracts;

namespace Authorization.Domain.Types;

public class Token : CustomType<string, Token>, IIdentity
{
}