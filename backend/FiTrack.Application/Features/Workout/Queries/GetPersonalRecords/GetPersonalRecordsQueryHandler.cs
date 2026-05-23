using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FiTrack.Application.DTOs.Workout;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Exceptions;

namespace FiTrack.Application.Features.Workout.Queries.GetPersonalRecords;

public class GetPersonalRecordsQueryHandler(
    IActivityRepository activityRepository,
    ICurrentUserService currentUserService) : IRequestHandler<GetPersonalRecordsQuery, PersonalRecordsDto>
{
    public async Task<PersonalRecordsDto> Handle(GetPersonalRecordsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new DomainException("User is not authenticated.");
        }

        var activities = await activityRepository.GetByUserIdAsync(userId, cancellationToken);

        if (!activities.Any())
        {
            return new PersonalRecordsDto();
        }

        return new PersonalRecordsDto
        {
            LongestDistanceMeters = activities.Max(a => a.DistanceMeters),
            FastestPace = activities.Where(a => a.AveragePace.HasValue).Select(a => a.AveragePace!.Value).DefaultIfEmpty(0).Max(),
            LongestDurationSeconds = activities.Max(a => a.DurationSeconds),
            HighestElevationGainMeters = activities.Where(a => a.ElevationGainMeters.HasValue).Select(a => a.ElevationGainMeters!.Value).DefaultIfEmpty(0).Max()
        };
    }
}
