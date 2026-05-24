using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FiTrack.Domain.Exceptions;
using FluentValidation; // PASTIKAN USING INI ADA
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FiTrack.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Terjadi exception yang tidak tertangani.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            // 1. TANGKAP ERROR VALIDASI DARI FLUENT VALIDATION
            case ValidationException validationException:
                statusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Errors
                    .Select(e => new { Field = e.PropertyName, Message = e.ErrorMessage });
                result = JsonSerializer.Serialize(new { Message = "Validasi Gagal", Errors = errors });
                break;

            // 2. ERROR LOGIKA BISNIS KITA
            case DomainException domainException:
                statusCode = (int)HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { Message = domainException.Message });
                break;

            case UnauthorizedAccessException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new { Message = "Sesi tidak valid atau telah berakhir." });
                break;

            case KeyNotFoundException keyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { Message = keyNotFoundException.Message });
                break;

            default:
                result = JsonSerializer.Serialize(new { Message = "Terjadi kesalahan internal pada server." });
                break;
        }

        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
}