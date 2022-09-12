using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimeKeepr.Application.Common.Interfaces;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Infrastructure.Services;

namespace TimeKeepr.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SymmetricKeyService _symmetricKeyService;

        public IdentityService(IWebHostEnvironment env,
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            SymmetricKeyService symmetricKeyService)
        {
            _env = env;
            _config = config;
            _userManager = userManager;
            _symmetricKeyService = symmetricKeyService;
        }

        public async Task<(bool, string)> RegisterUserAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                HireDate = registerDto.HireDate.Date.ToUniversalTime(),
                VacationDaysAccruedPerMonth = registerDto.VacationDaysAccruedPerMonth,
                SickHoursAccruedPerMonth = registerDto.SickHoursAccruedPerMonth,
                PersonalDaysPerYear = registerDto.PersonalDaysPerYear
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "General");
                return (true, user.Id);
            }

            return (false, result.Errors.FirstOrDefault().Description);
        }

        public async Task<(bool, string)> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return (false, "User does not exist.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                return (false, "Invalid password.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, loginDto.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("HireDate", user.HireDate.ToString()),
                new Claim("VacationDaysAccruedPerMonth", user.VacationDaysAccruedPerMonth.ToString()),
                new Claim("SickHoursAccruedPerMonth", user.SickHoursAccruedPerMonth.ToString()),
                new Claim("PersonalDaysPerYear", user.PersonalDaysPerYear.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var symmetricKey = _symmetricKeyService.GetSymmetricKey();

            string issuer = _config["TimeKeepr:Issuer"];
            string audience = _config["TimeKeepr:Audience"];

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(14),
                signingCredentials: new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (true, tokenString);
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<(bool, string)> UpdateUserAsync(UpdateApplicationUserDto userDto)
        {
            var thisUser = await _userManager.Users.FirstAsync(u => u.Id == userDto.Id);

            thisUser.FirstName = userDto.FirstName;
            thisUser.LastName = userDto.LastName;
            thisUser.HireDate = userDto.HireDate.Date.ToUniversalTime();
            thisUser.VacationDaysAccruedPerMonth = userDto.VacationDaysAccruedPerMonth;
            thisUser.SickHoursAccruedPerMonth = userDto.SickHoursAccruedPerMonth;
            thisUser.PersonalDaysPerYear = userDto.PersonalDaysPerYear;

            var result = await _userManager.UpdateAsync(thisUser);

            if (result.Succeeded)
            {
                return (true, thisUser.Id);
            }

            return (false, result.Errors.FirstOrDefault().Description);
        }

        public async Task<(bool, IEnumerable<ApplicationUserDto>)> GetAllUsersAsync()
        {
            var allUsers = await _userManager.Users
                .Select(u => new ApplicationUserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    HireDate = u.HireDate,
                    VacationDaysAccruedPerMonth = u.VacationDaysAccruedPerMonth,
                    SickHoursAccruedPerMonth = u.SickHoursAccruedPerMonth,
                    PersonalDaysPerYear = u.PersonalDaysPerYear
                }).ToListAsync();

            if (allUsers.Any())
            {
                return (true, allUsers);
            }

            return (false, new List<ApplicationUserDto>());
        }
    }
}
