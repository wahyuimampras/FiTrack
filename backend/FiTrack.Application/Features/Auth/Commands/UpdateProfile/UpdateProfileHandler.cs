using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateProfileCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null) throw new KeyNotFoundException("User not found.");

        user.UpdateProfile(request.Username, request.Email);

        await userRepository.UpdateAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}