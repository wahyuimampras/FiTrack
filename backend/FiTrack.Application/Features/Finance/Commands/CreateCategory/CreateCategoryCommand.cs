using MediatR;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Domain.Enums;

namespace FiTrack.Application.Features.Finance.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    CategoryType Type,
    string? Icon,
    string? Color
) : IRequest<CategoryDto>;