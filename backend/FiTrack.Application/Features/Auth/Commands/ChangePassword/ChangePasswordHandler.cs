using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Exceptions; // PENTING UNTUK DOMAIN EXCEPTION
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUserService) : IRequestHandler<ChangePasswordCommand, Unit>
{
    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user == null) throw new KeyNotFoundException("User not found.");

        if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
        {
            throw new DomainException("Password lama yang Anda masukkan salah.");
        }

        var newHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatePassword(newHash);

        await userRepository.UpdateAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}