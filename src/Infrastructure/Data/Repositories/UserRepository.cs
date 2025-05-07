using Domain.Enums;
using HireHive.Domain.Entities;
using HireHive.Domain.Exceptions.User;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public UserRepository(
        UserManager<User> userManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<User> AddAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception("Failed to create user.");

        var roleResult = await _userManager.AddToRoleAsync(user, Roles.Candidate.ToString());
        if (!roleResult.Succeeded)
            throw new Exception("Failed to assign role to user.");

        return user;
    }

    public async Task DeleteAsync(User user)
    {
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new Exception("Failed to delete user.");
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _context.Users
            .Where(u => !_context.UserRoles
                .Any(ur => ur.UserId == u.Id &&
                            _context.Roles
                            .Any(r => r.Id == ur.RoleId && r.Name == Roles.Admin.ToString())))
            .ToListAsync();

        return users;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new UserNotFoundException();

        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception("Failed to update user.");
    }
}
