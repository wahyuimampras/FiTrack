using MediatR;

namespace FiTrack.Application.Features.Workout.Queries.GetStravaAuthUrl;

public class GetStravaAuthUrlQuery : IRequest<string>
{
    public string RedirectUri { get; set; } = string.Empty;
}