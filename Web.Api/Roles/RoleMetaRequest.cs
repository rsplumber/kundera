using Application.Roles;
using Core.Domains.Roles.Types;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Roles;

public record AddRoleMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public ChangeRoleMetaCommand ToCommand(Guid roleId) => new(RoleId.From(roleId), Meta);
}

public class AddRoleMetaRequestValidator : RequestValidator<AddRoleMetaRequest>
{
    public AddRoleMetaRequestValidator()
    {
        RuleFor(request => request.Meta)
            .NotEmpty()
            .WithMessage("Enter valid Meta")
            .NotNull()
            .WithMessage("Enter valid Meta");
    }
}

public record RemoveRoleMetaRequest(string[] MetaKeys) : IWebRequest
{
    public RemoveRoleMetaCommand ToCommand(Guid roleId) => new(RoleId.From(roleId), MetaKeys);
}

public class RemoveRoleMetaRequestValidator : RequestValidator<RemoveRoleMetaRequest>
{
    public RemoveRoleMetaRequestValidator()
    {
        RuleFor(request => request.MetaKeys)
            .NotEmpty()
            .WithMessage("Enter valid Meta")
            .NotNull()
            .WithMessage("Enter valid Meta");
    }
}