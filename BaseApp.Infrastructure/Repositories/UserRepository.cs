using BaseApp.Domain.Interfaces;
using BaseApp.Domain.Models;
using BaseApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}
