using MediatR;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string? Icon,
    string? Color
) : IRequest<CategoryDto>;