using System.Threading;
using System.Threading.Tasks;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Workout.Queries.GetStravaAuthUrl;

public class GetStravaAuthUrlQueryHandler : IRequestHandler<GetStravaAuthUrlQuery, string>
{
    private readonly IStravaService _stravaService;

    public GetStravaAuthUrlQueryHandler(IStravaService stravaService)
    {
        _stravaService = stravaService;
    }

    public Task<string> Handle(GetStravaAuthUrlQuery request, CancellationToken cancellationToken)
    {
        var url = _stravaService.GetAuthUrl(request.RedirectUri);
        return Task.FromResult(url);
    }
}