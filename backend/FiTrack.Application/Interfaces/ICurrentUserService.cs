namespace FiTrack.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
}