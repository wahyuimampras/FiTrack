using FiTrack.Application.DTOs.Auth;
using MediatR;

namespace FiTrack.Application.Features.Auth.Queries.GetUserProfile;

public record GetUserProfileQuery() : IRequest<UserProfileDto>;