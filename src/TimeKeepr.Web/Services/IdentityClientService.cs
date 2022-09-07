using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using TimeKeepr.Application.Identity.Dtos;
using TimeKeepr.Web.Common;
using TimeKeepr.Web.Providers;

namespace TimeKeepr.Web.Services
{
    public class IdentityClientService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public IdentityClientService(HttpClient client, AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<IdentityRequestResult> Register(RegisterDto registerDto)
        {
            var response = await _client.PostAsJsonAsync("identity/register", registerDto);
            var authResult = await response.Content.ReadFromJsonAsync<IdentityRequestResult>();

            if (authResult is not null)
            {
                if (authResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(authResult.Token))
                    {
                        await _localStorage.SetItemAsync("authToken", authResult.Token);
                        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.Token);
                        ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
                    }
                }

                return authResult;
            }

            return IdentityRequestResult.Failure(new string[] { "There was an error during registration." });
        }

        public async Task<IdentityRequestResult> Login(LoginDto loginDto)
        {
            var response = await _client.PostAsJsonAsync("identity/login", loginDto);
            var authResult = await response.Content.ReadFromJsonAsync<IdentityRequestResult>();

            if (authResult is not null)
            {
                if (authResult.Succeeded)
                {
                    await _localStorage.SetItemAsync("authToken", authResult.Token);
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authResult.Token);
                    ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserAuthentication(authResult.Token);
                }

                return authResult;
            }

            return IdentityRequestResult.Failure(new string[] { "There was an error during login." });
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _client.DefaultRequestHeaders.Authorization = null;
            ((TokenAuthenticationStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<IdentityRequestResult> UpdateUser(UpdateApplicationUserDto userDto)
        {
            var response = await _client.PutAsJsonAsync("identity/update", userDto);
            var result = await response.Content.ReadFromJsonAsync<IdentityRequestResult>();

            if (result is not null)
            {
                return result;
            }

            return IdentityRequestResult.Failure(new string[] { "There was an error updating." });
        }

        public async Task<ApplicationUserDto> GetUserDetails()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            var claims = JwtParser.ParseClaimsFromJwt(token);

            var user = new ApplicationUserDto
            {
                Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
                FirstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value,
                LastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value,
                HireDate = DateTime.Parse(claims.FirstOrDefault(c => c.Type == "HireDate").Value),
                VacationDaysAccruedPerMonth = decimal.Parse(claims.FirstOrDefault(c => c.Type == "VacationDaysAccruedPerMonth").Value),
                SickHoursAccruedPerMonth = decimal.Parse(claims.FirstOrDefault(c => c.Type == "SickHoursAccruedPerMonth").Value),
                PersonalDaysPerYear = int.Parse(claims.FirstOrDefault(c => c.Type == "PersonalDaysPerYear").Value)
            };

            return user;
        }
    }
}
