using Core;
using FastEndpoints;
using FluentValidation;

namespace Application;

public sealed class ExceptionHandlerMiddleware : IMiddleware
{
    private const int InternalServerErrorCode = 500;
    private const string InternalServerErrorMessage = "Whoops :( , somthing impossibly went wrong!";

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            return next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            return exception switch
            {
                CoreException coreException => response.SendAsync(coreException.Message, coreException.Code),
                ValidationException validationException => response.SendAsync(string.Join(", ", validationException.Errors.Select(failure => $"{failure.PropertyName} : {failure.ErrorMessage}")), 400),
                _ => response.SendAsync(InternalServerErrorMessage, InternalServerErrorCode)
            };
        }
    }
}