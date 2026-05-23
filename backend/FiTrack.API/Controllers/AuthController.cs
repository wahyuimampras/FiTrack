using FiTrack.Application.Features.Auth.Commands.Login;
using FiTrack.Application.Features.Auth.Commands.Register;
using FiTrack.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender mediator, ITokenService tokenService, ISessionService sessionService) : ControllerBase
{
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

        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new
        {
            accessToken,
            expiresIn = 900,
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

        // Rotate refresh token
        var newRefreshToken = tokenService.GenerateRefreshToken();
        var newAccessToken = tokenService.GenerateAccessToken(session.User, session.Id);

        await sessionService.RotateRefreshTokenAsync(session.Id, newRefreshToken);

        Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        });

        return Ok(new { accessToken = newAccessToken, expiresIn = 900 });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (refreshToken != null)
        {
            var session = await sessionService.ValidateRefreshTokenAsync(refreshToken);
            if (session != null) await sessionService.RevokeSessionAsync(session.Id);
        }

        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "Logged out successfully" });
    }
}