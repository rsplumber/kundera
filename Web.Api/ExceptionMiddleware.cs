using Auth.Application.Authentication;
using Auth.Application.Authorization;
using FluentValidation;
using Kite.Domain.Contracts;
using Kite.Serializer;
using Microsoft.AspNetCore.Http;

namespace Web.Api;

internal sealed class ExceptionMiddleware : IMiddleware
{
    private const string InternalServerErrorMessage = "Whoops! something wnet wrong :(";
    private readonly ISerializerService _jsonSerializer;

    public ExceptionMiddleware(ISerializerService jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

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
            var message = exception.Message;
            var code = exception switch
            {
                DomainException or ValidationException => StatusCodes.Status400BadRequest,
                UnAuthenticateException => StatusCodes.Status401Unauthorized,
                UnAuthorizedException => StatusCodes.Status403Forbidden,
                _ => 0
            };
            if (code == 0 && exception.GetType().Name.EndsWith("NotFoundException"))
            {
                code = StatusCodes.Status404NotFound;
            }

            if (code == 0)
            {
                code = StatusCodes.Status500InternalServerError;
            }

            if (code == StatusCodes.Status500InternalServerError)
            {
                message = InternalServerErrorMessage;
            }

            response.StatusCode = code;
            var result = _jsonSerializer.Serialize(new ExceptionResponse(message));
            await response.WriteAsync(result);
        }
    }
}

internal sealed record ExceptionResponse(string Message);