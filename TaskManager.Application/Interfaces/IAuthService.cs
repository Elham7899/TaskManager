using TaskManager.API.DTOs;
using TaskManager.Application.DTOs.Login;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface IAuthService
{
    string GenerateJwtToken(ApplicationUser user);
    Task<string> RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
}
