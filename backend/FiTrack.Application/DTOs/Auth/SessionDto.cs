using System;

namespace FiTrack.Application.DTOs.Auth;

public record SessionDto(
    Guid Id,
    string DeviceInfo,
    string IpAddress,
    bool IsActive,
    DateTime CreatedAt,
    DateTime LastAccessedAt
);