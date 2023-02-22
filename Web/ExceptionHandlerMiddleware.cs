﻿using System.Text.Json;
using Core;
using FluentValidation;

namespace Web;

public class ExceptionHandlerMiddleware : IMiddleware
{
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
            string message;
            switch (exception)
            {
                case KunderaException kunderaException:
                    response.StatusCode = kunderaException.Code;
                    message = kunderaException.Message;
                    break;
                case ValidationException validationException:
                    response.StatusCode = 400;
                    message = string.Join(", ", validationException.Errors
                        .Select(failure => $"{failure.PropertyName} : {failure.ErrorMessage}"));
                    break;
                default:
                    response.StatusCode = 500;
                    message = InternalServerErrorMessage;
                    break;
            }

            await response.WriteAsync(JsonSerializer.Serialize(new
            {
                message
            }));
        }
    }
}