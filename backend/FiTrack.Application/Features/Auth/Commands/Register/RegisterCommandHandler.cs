using MediatR;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Domain.Exceptions;

namespace FiTrack.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(IUserRepository userRepository) : IRequestHandler<RegisterCommand, User>
{
    public async Task<User> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existingUserByEmail = await userRepository.GetByEmailAsync(request.Email, ct);
        if (existingUserByEmail != null)
            throw new DomainException("Email is already in use");

        var existingUserByUsername = await userRepository.GetByUsernameAsync(request.Username, ct);
        if (existingUserByUsername != null)
            throw new DomainException("Username is already taken");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = User.Create(request.Username, request.Email, passwordHash);

        await userRepository.AddAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);

        return user;
    }
}