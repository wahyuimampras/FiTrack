using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.ChangePassword;

public record ChangePasswordCommand(string OldPassword, string NewPassword) : IRequest<Unit>;