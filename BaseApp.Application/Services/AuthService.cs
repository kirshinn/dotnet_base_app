using BaseApp.Domain.Interfaces;
using BaseApp.Domain.Models;
using BaseApp.Application.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace BaseApp.Application.Services;

public class AuthService(IUserRepository userRepository)
{
    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null) return false;

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            Password = HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user);
        return true;
    }

    public async Task<User?> LoginAsync(LoginDto dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !VerifyPassword(dto.Password, user.Password))
            return null;
        return user;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string hash)
    {
        var computedHash = HashPassword(password);
        return computedHash == hash;
    }
}
