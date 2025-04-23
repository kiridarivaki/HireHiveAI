using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public UserRepository(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Guid> AddUserAsync(DomainUser user)
    {
        var appUser = _mapper.Map<AppUser>(user);
        await _userManager.CreateAsync(appUser, "Hello1!@");

        return user.Id;
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        await _userManager.DeleteAsync(appUser);
    }

    public async Task<IEnumerable<DomainUser>> GetAllUsersAsync()
    {
        var appUsers = await _userManager.Users.ToListAsync();

        return _mapper.Map<List<DomainUser>>(appUsers);
    }

    public async Task<DomainUser> GetByIdAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());

        return _mapper.Map<DomainUser>(appUser);
    }

    public async Task UpdateUserAsync(DomainUser user)
    {
        await _userManager.UpdateAsync(_mapper.Map<AppUser>(user));
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user != null;
    }
}
