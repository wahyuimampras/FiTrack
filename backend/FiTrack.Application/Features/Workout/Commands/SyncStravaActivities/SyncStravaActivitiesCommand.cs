using MediatR;
using FiTrack.Domain.Entities;

namespace FiTrack.Application.Features.Workout.Commands.SyncStravaActivities;

public record SyncStravaActivitiesCommand : IRequest<List<Activity>>;