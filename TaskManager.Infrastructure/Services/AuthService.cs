using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.API.DTOs;
using TaskManager.Application.DTOs.Login;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Infrastructure.Services;

public class AuthService(IConfiguration config, ApplicationDbContext _context) : IAuthService
{
    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var keyBytes = Encoding.UTF8.GetBytes(config["Jwt:Key"]!);
        if (keyBytes.Length < 32)
        {
            Array.Resize(ref keyBytes, 32);
        }
        var key = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
        if ((user is null) || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            throw new Exception("Invalid username or password");

        return GenerateJwtToken(user);
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (_context.Users.Any(u => u.Username == dto.Username))
            throw new Exception("Username already exists");

        var user = new User
        {
            Username = dto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return GenerateJwtToken(user);
    }
}
