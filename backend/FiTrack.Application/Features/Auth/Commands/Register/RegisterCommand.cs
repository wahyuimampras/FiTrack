using MediatR;
using FiTrack.Domain.Entities;

namespace FiTrack.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Username, string Email, string Password) : IRequest<User>;