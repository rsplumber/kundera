using Tes.Domain.Contracts;

namespace Auth.Domain.Sessions;

public class Token : CustomType<string, Token>, IIdentity
{
}