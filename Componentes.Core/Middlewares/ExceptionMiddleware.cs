using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mime;
using Componentes.Models.Exceptions;
using Componentes.Models.Responses;
using Componentes.Utilities.Extensions;
using Microsoft.AspNetCore.Http;

namespace Componentes.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError($"[ExceptionMiddleware] | InvokeAsync | ERROR: {e.ToJsonString()}");
            await HandleExceptionAsync(context, e);
        }
    }


    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string exceptionBody;

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case CustomException customException:
                context.Response.StatusCode = (int)customException.StatusCode;
                exceptionBody = customException.ExceptionBody ?? string.Empty;

                return context.Response.WriteAsync(exceptionBody);
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                exceptionBody = exception.Message;

                var response = new ServicesResponse
                {
                    Success = false,
                    Code = context.Response.StatusCode,
                    Message = exceptionBody
                };

                return context.Response.WriteAsync(response.ToJsonString());
        }
    }
}