using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Auth;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Auth.Queries.GetUserProfile;

public class GetUserProfileHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetUserProfileQuery, UserProfileDto>
{
    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null) throw new KeyNotFoundException("User profile not found.");

        return mapper.Map<UserProfileDto>(user);
    }
}