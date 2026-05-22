using MediatR;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;

namespace FiTrack.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(IUserRepository userRepository) : IRequestHandler<LoginCommand, User?>
{
    public async Task<User?> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, ct);
        
        if (user == null)
        {
            return null;
        }

        // Using BCrypt from BCrypt.Net-Next as mentioned in PANDUAN_FITRACK_APP.md
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        return user;
    }
}