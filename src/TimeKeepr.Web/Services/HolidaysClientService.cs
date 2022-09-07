using System.Net.Http.Json;
using TimeKeepr.Application.Common.Dtos;
using TimeKeepr.Application.Holidays.Dtos;

namespace TimeKeepr.Web.Services
{
    public class HolidaysClientService
    {
        private readonly HttpClient _client;

        public HolidaysClientService(HttpClient client)
        {
            _client = client;
        }

        public async Task<RequestResult<IEnumerable<HolidayDto>>> GetByYear(int year)
        {
            var response = await _client.GetFromJsonAsync<RequestResult<IEnumerable<HolidayDto>>>($"holidays/{year}");

            if (response is not null)
            {
                return response;
            }

            return RequestResult<IEnumerable<HolidayDto>>.Failure(new string[] { "There was an issue retrieving the requested data." });
        }

        public async Task<RequestResult<HolidayDto>> GetyById(int id)
        {
            var response = await _client.GetFromJsonAsync<RequestResult<HolidayDto>>($"holidays/{id}");

            if (response is not null)
            {
                return response;
            }

            return RequestResult<HolidayDto>.Failure(new string[] { "There was an issue retrieving the requested data." });
        }

        public async Task<RequestResult<HolidayDto>> Create(CreateHolidayDto createHolidayDto)
        {
            var response = await _client.PostAsJsonAsync("holidays", createHolidayDto);
            var result = await response.Content.ReadFromJsonAsync<RequestResult<HolidayDto>>();

            if (result is not null)
            {
                return result;
            }

            return RequestResult<HolidayDto>.Failure(new string[] { "There was an issue creating the holiday." });
        }

        public async Task<bool> Update(int id, UpdateHolidayDto updateHolidayDto)
        {
            var response = await _client.PutAsJsonAsync($"holidays/{id}", updateHolidayDto);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _client.DeleteAsync($"holidays/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }
    }
}
