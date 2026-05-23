using MediatR;

namespace FiTrack.Application.Features.Workout.Commands.ExchangeStravaToken;

public class ExchangeStravaTokenCommand : IRequest<bool>
{
    public string Code { get; set; } = string.Empty;
}