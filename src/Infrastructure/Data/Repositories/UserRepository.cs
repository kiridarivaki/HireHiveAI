using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;

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

    public async Task AddUser(DomainUser user)
    {
        var appUser = _mapper.Map<AppUser>(user);
        await _userManager.CreateAsync(appUser, "Hello1!@");
    }

    public async Task DeleteUser(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        if (appUser == null) throw new Exception("User not found");

        await _userManager.DeleteAsync(appUser);
    }

    public async Task<IEnumerable<DomainUser>> GetAllUsersAsync()
    {
        var appUsers = await _userManager.Users.ToListAsync();
        if (appUsers == null) return Enumerable.Empty<DomainUser>();

        return _mapper.Map<IEnumerable<DomainUser>>(appUsers);
    }

    public async Task<DomainUser> GetByIdAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString());
        if (appUser == null) throw new ArgumentException();

        return _mapper.Map<DomainUser>(appUser);
    }

    public async Task UpdateUser(DomainUser user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser == null) throw new Exception("User not found");

        _mapper.Map(user, appUser);
        await _userManager.UpdateAsync(appUser);
    }
}
