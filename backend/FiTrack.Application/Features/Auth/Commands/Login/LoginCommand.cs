using MediatR;
using FiTrack.Domain.Entities;

namespace FiTrack.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Username, string Password) : IRequest<User?>;