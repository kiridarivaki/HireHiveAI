using Domain.Enums;
using HireHive.Domain.Entities;
using HireHive.Domain.Enums;
using HireHive.Domain.Exceptions.User;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly HireHiveDbContext _context;

    public UserRepository(
        UserManager<User> userManager,
        HireHiveDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<User> Add(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception("Failed to create user.");

        var roleResult = await _userManager.AddToRoleAsync(user, Roles.Candidate.ToString());
        if (!roleResult.Succeeded)
            throw new Exception("Failed to assign role to user.");

        return user;
    }

    public async Task Delete(User user)
    {
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new Exception("Failed to delete user.");
    }

    public async Task<List<User>> GetAll()
    {
        var users = await _context.Users
            .Where(u => !_context.UserRoles
                .Any(ur => ur.UserId == u.Id &&
                            _context.Roles
                            .Any(r => r.Id == ur.RoleId && r.Name == Roles.Admin.ToString())))
            .ToListAsync();

        return users;
    }

    public async Task<List<User>> GetByIds(List<Guid> userIds)
    {
        var users = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        return users;
    }

    public async Task<User> GetById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            throw new UserNotFoundException();

        return user;
    }

    public async Task Update(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new Exception("Failed to update user.");
    }

    public int CountFiltered(JobType jobType)
    {
        return _context.Users
            .Where(u => u.JobTypes.Contains(jobType))
            .Count();
    }
}
