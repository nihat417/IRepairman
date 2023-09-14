using IRepairman.Domain.Entities;

namespace IRepairman.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CreateUserAsync(AppUser user, string password);
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<bool> IsEmailConfirmedAsync(AppUser user);
        Task<bool> ConfirmEmailAsync(AppUser user, string token);
        Task<bool> ResetPasswordAsync(AppUser user, string token, string newPassword);
        Task<bool> IsInRoleAsync(AppUser user, string roleName);
        Task<string> GenerateToken(AppUser user);
    }

}
