using MediatR;
using FiTrack.Application.DTOs.Workout;

namespace FiTrack.Application.Features.Workout.Queries.GetPersonalRecords;

public class GetPersonalRecordsQuery : IRequest<PersonalRecordsDto>
{
}
