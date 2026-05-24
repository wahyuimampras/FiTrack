using System;

namespace FiTrack.Application.DTOs.Auth;

public record UserProfileDto(
    Guid Id,
    string Username,
    string Email
);