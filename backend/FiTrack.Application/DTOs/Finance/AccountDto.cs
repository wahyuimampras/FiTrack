using System;

namespace FiTrack.Application.DTOs.Finance;

public record AccountDto(
    Guid Id, 
    string Name, 
    string Type, 
    decimal Balance, 
    string? Color, 
    string? Icon, 
    bool IsActive);