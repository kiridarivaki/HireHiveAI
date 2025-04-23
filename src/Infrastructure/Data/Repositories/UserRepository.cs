using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        UserManager<AppUser> userManager,
        AppDbContext context, IMapper mapper,
        ILogger<UserRepository> logger)
    {
        _userManager = userManager;
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> AddUserAsync(User user, string password)
    {
        var appUser = _mapper.Map<AppUser>(user);
        await _userManager.CreateAsync(appUser, password);

        return appUser.Id;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        await _userManager.DeleteAsync(appUser);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var appUsers = await _context.Users.Where(u => _context.UserRoles
                .Any(ur => ur.UserId == u.Id &&
                    _context.Roles.Any(r => r.Id == ur.RoleId && r.Name == Roles.Candidate.ToString())))
                .ToListAsync();

        return _mapper.Map<List<User>>(appUsers);
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());

        return _mapper.Map<User>(appUser);
    }

    public async Task UpdateUserAsync(User user)
    {
        // todo: remove double check for user in app and infra layer 
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser == null) throw new Exception("User not found");

        _mapper.Map(user, appUser);

        var result = await _userManager.UpdateAsync(appUser);
        if (!result.Succeeded)
        {
            throw new Exception("Failed to update user.");
        }
    }
}
