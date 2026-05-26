using FiTrack.Application.Features.Auth.Commands.Login;
using FiTrack.Application.Features.Auth.Commands.Register;
using FiTrack.Domain.Interfaces.Services;
using FiTrack.Application.Features.Auth.Commands.Logout;
using FiTrack.Application.Features.Auth.Commands.RevokeSession;
using FiTrack.Application.Features.Auth.Commands.RevokeAllSessions;
using FiTrack.Application.Features.Auth.Queries.GetSessions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    ISender mediator,
    ITokenService tokenService,
    ISessionService sessionService,
    IWebHostEnvironment env) : ControllerBase
{
    // Helper: gunakan Secure=false di development (localhost HTTP)
    private CookieOptions RefreshTokenCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = !env.IsDevelopment(),          // false saat dev, true saat production
        SameSite = env.IsDevelopment()
            ? SameSiteMode.Lax                  // Lax agar bisa dikirim cross-port di localhost
            : SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddDays(30)   // 30 hari
    };

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var user = await mediator.Send(command);
        return Ok(new { message = "Registration successful", userId = user.Id });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var user = await mediator.Send(command);
        if (user == null) return Unauthorized(new { message = "Invalid credentials" });

        var refreshToken = tokenService.GenerateRefreshToken();
        var deviceInfo = Request.Headers.UserAgent.ToString();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var session = await sessionService.CreateSessionAsync(user.Id, refreshToken, deviceInfo, ip);
        var accessToken = tokenService.GenerateAccessToken(user, session.Id);

        Response.Cookies.Append("refreshToken", refreshToken, RefreshTokenCookieOptions());

        return Ok(new
        {
            accessToken,
            expiresIn = 7200,  // 2 jam
            user = new { user.Id, user.Username, user.Email, StravaConnected = user.StravaAthleteId != null }
        });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "No refresh token" });

        var session = await sessionService.ValidateRefreshTokenAsync(refreshToken);
        if (session == null)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        var newRefreshToken = tokenService.GenerateRefreshToken();
        var newAccessToken = tokenService.GenerateAccessToken(session.User, session.Id);

        await sessionService.RotateRefreshTokenAsync(session.Id, newRefreshToken);

        Response.Cookies.Append("refreshToken", newRefreshToken, RefreshTokenCookieOptions());

        return Ok(new { accessToken = newAccessToken, expiresIn = 7200 });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command, CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        Response.Cookies.Delete("refreshToken");
        return NoContent();
    }

    [Authorize]
    [HttpGet("sessions")]
    public async Task<IActionResult> GetSessions(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSessionsQuery(), cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("sessions/{id}")]
    public async Task<IActionResult> RevokeSession(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new RevokeSessionCommand(id), cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("sessions")]
    public async Task<IActionResult> RevokeAllSessions(CancellationToken cancellationToken)
    {
        await mediator.Send(new RevokeAllSessionsCommand(), cancellationToken);
        return NoContent();
    }
}