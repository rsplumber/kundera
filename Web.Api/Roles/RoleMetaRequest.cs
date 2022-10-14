using Application.Roles;
using Domain.Roles;
using FluentValidation;
using Kite.Web.Requests;

namespace Web.Api.Roles;

public record AddRoleMetaRequest(Dictionary<string, string> Meta) : IWebRequest
{
    public AddRoleMetaCommand ToCommand(string roleId) => new(RoleId.From(roleId), Meta);
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
    public RemoveRoleMetaCommand ToCommand(string roleId) => new(RoleId.From(roleId), MetaKeys);
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