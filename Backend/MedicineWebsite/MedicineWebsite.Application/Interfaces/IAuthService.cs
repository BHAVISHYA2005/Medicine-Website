using MedicineWebsite.Shared.DTOs.Auth;

namespace MedicineWebsite.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<bool> ConfirmEmailAsync(string userId, string token);
    Task<bool> ForgotPasswordAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<UserDto?> GetUserProfileAsync(string userId);
    Task<bool> UpdateUserProfileAsync(string userId, UserDto userDto);
}
