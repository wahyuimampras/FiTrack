using System;
using System.Threading.Tasks;
using FiTrack.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FiTrack.API.Middleware;

public class SessionValidationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ISessionService sessionService)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            await next(context);
            return;
        }

        if (context.User.Identity?.IsAuthenticated == true)
        {
            // By default, TokenService should embed the sessionId in claims
            var sessionIdClaim = context.User.FindFirst("sessionId")?.Value;
            if (sessionIdClaim != null && Guid.TryParse(sessionIdClaim, out var sessionId))
            {
                var isValid = await sessionService.IsSessionActiveAsync(sessionId);
                if (!isValid)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(new { message = "Session expired or revoked" });
                    return;
                }
            }
        }

        await next(context);
    }
}
