using TimeKeepr.Application.Identity.Dtos;

namespace TimeKeepr.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool, string)> RegisterUserAsync(RegisterDto registerDto);

        Task<(bool, string)> LoginUserAsync(LoginDto loginDto);

        Task<string> GetUserNameAsync(string userId);

        Task<(bool, string)> UpdateUserAsync(UpdateApplicationUserDto userDto);
    }
}
