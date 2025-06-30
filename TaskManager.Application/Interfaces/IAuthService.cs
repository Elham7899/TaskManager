using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface IAuthService
{
    string GenerateJwtToken(User user);
}
