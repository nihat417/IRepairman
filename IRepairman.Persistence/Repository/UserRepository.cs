using IRepairman.Application.Interfaces;
using IRepairman.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly UserManager<AppUser> userManager;

    public UserRepository(UserManager<AppUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<bool> ConfirmEmailAsync(AppUser user, string token)
    {
        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<bool> CreateUserAsync(AppUser user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded;
    }

    public async Task<string> GenerateToken(AppUser user)
    {
        return await userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await userManager.FindByNameAsync(username);
    }

    public async Task<bool> IsEmailConfirmedAsync(AppUser user)
    {
        return await userManager.IsEmailConfirmedAsync(user);
    }

    public async Task<bool> IsInRoleAsync(AppUser user, string roleName)
    {
        return await userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<IdentityResult> ResetPasswordsAsync(AppUser user, string token, string newPassword)
    {
        return await userManager.ResetPasswordAsync(user, token, newPassword);
        
    }

    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        return await userManager.Users.ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(string id)
    {
        return await userManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> UpdateAsync(AppUser user)
    {
        if (user != null)
        {
            var result = await userManager.UpdateAsync(user);
            return result;
        }
        return IdentityResult.Failed();
    }
}
