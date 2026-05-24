using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.UpdateProfile;

public record UpdateProfileCommand(string Username, string Email) : IRequest<Unit>;