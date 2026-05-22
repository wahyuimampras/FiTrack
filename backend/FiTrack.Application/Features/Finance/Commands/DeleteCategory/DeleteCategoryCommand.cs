using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest<Unit>;