using AutoMapper;
using Domain.Enums;
using HireHive.Domain.Entities;
using HireHive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HireHive.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        UserManager<User> userManager,
        AppDbContext context, IMapper mapper,
        ILogger<UserRepository> logger)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<User> AddUserAsync(User user, string password)
    {
        await _userManager.CreateAsync(user, password);
        return user;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        await _userManager.DeleteAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var appUsers = await _context.Users.Where(u => _context.UserRoles
                .Any(ur => ur.UserId == u.Id &&
                    _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == Roles.Candidate.ToString())))
                .ToListAsync();

        return _mapper.Map<List<User>>(appUsers);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task UpdateUserAsync(User user)
    {
        // todo: remove double check for user in app and infra layer 
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to update user.");
        }
    }
}
