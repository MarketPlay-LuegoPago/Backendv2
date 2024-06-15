// File: Errors/ErrorHandlingMiddleware.cs
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        string message;

        switch (ex)
        {
            case ArgumentException:
                statusCode = (int)HttpStatusCode.BadRequest;
                message = "Invalid request.";
                break;
            case InvalidOperationException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = "Resource not found.";
                break;
            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "Internal server error.";
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new { status = statusCode, message = message };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
