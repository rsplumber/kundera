using Core;
using FastEndpoints;
using FluentValidation;

namespace Application;

public sealed class ExceptionHandlerMiddleware : IMiddleware
{
    private const int InternalServerErrorCode = 500;
    private const string InternalServerErrorMessage = "Whoops :( , somthing impossibly went wrong!";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            switch (exception)
            {
                case CoreException coreException:
                    await response.SendAsync(coreException.Message, coreException.Code);
                    break;
                case ValidationException validationException:
                    await response.SendAsync(string.Join(", ", validationException.Errors.Select(failure => $"{failure.PropertyName} : {failure.ErrorMessage}")), 400);
                    break;
                default:
                    await response.SendAsync(InternalServerErrorMessage, InternalServerErrorCode);
                    break;
            }
        }
    }
}