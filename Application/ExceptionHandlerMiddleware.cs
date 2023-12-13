using Core;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace Application;

public sealed class ExceptionHandlerMiddleware : IExceptionHandler
{
    private const int InternalServerErrorCode = 500;
    private const string InternalServerErrorMessage = "Whoops :( , somthing impossibly went wrong!";


    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var response = httpContext.Response;
        response.ContentType = "application/json";
        switch (exception)
        {
            case CoreException coreException:
                await response.SendAsync(coreException.Message, coreException.Code, cancellation: cancellationToken);
                break;
            case ValidationException validationException:
                await response.SendAsync(string.Join(", ", validationException.Errors.Select(failure => $"{failure.PropertyName} : {failure.ErrorMessage}")), 422, cancellation: cancellationToken);
                break;
            default:
                await response.SendAsync(InternalServerErrorMessage, InternalServerErrorCode, cancellation: cancellationToken);
                break;
        }

        return true;
    }
}