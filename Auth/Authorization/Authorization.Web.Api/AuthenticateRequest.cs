using FluentValidation;
using Tes.Web.Validators;

namespace Authorization.Web.Api;

public record CertificateRequest(string OneTimeToken) : IWebRequest;

public class CertificateRequestValidator : RequestValidator<CertificateRequest>
{
    public CertificateRequestValidator()
    {
        RuleFor(request => request.OneTimeToken)
            .NotEmpty().WithMessage("Enter valid OneTimeToken")
            .NotNull().WithMessage("Enter valid OneTimeToken");
    }
}