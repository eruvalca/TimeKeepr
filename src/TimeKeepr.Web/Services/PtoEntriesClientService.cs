using System.Net.Http.Json;
using System.Net;
using TimeKeepr.Application.PtoEntries.Dtos;
using TimeKeepr.Application.Common.Dtos;

namespace TimeKeepr.Web.Services
{
    public class PtoEntriesClientService
    {
        private readonly HttpClient _client;

        public PtoEntriesClientService(HttpClient client)
        {
            _client = client;
        }

        public async Task<RequestResult<IEnumerable<PtoEntryDto>>> GetByUserAndYear(string applicationUserId, int year)
        {
            var response = await _client
                .GetFromJsonAsync<RequestResult<IEnumerable<PtoEntryDto>>>($"ptoEntries/{applicationUserId}/{year}");

            if (response is not null)
            {
                return response;
            }

            return RequestResult<IEnumerable<PtoEntryDto>>.Failure(new string[] { "There was an issue retrieving the requested data." });
        }

        public async Task<RequestResult<PtoEntryDto>> GetyById(int id)
        {
            var response = await _client.GetFromJsonAsync<RequestResult<PtoEntryDto>>($"ptoEntries/{id}");

            if (response is not null)
            {
                return response;
            }

            return RequestResult<PtoEntryDto>.Failure(new string[] { "There was an issue retrieving the requested data." });
        }

        public async Task<RequestResult<PtoEntryDto>> Create(CreatePtoEntryDto createPtoEntryDto)
        {
            var response = await _client.PostAsJsonAsync("ptoEntries", createPtoEntryDto);
            var result = await response.Content.ReadFromJsonAsync<RequestResult<PtoEntryDto>>();

            if (result is not null)
            {
                return result;
            }

            return RequestResult<PtoEntryDto>.Failure(new string[] { "There was an issue creating the PTO entry." });
        }

        public async Task<RequestResult<int>> Update(int id, UpdatePtoEntryDto updatePtoEntryDto)
        {
            var response = await _client.PutAsJsonAsync($"ptoEntries/{id}", updatePtoEntryDto);

            if (!response.IsSuccessStatusCode)
            {
                return RequestResult<int>.Failure(new string[] { "There was an issue updating the PTO entry." });
            }

            return RequestResult<int>.Success(1);
        }

        public async Task<RequestResult<int>> Delete(int id)
        {
            var response = await _client.DeleteAsync($"ptoEntries/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RequestResult<int>.Failure(new string[] { "There was an issue deleting the PTO entry." });
            }

            return RequestResult<int>.Success(1);
        }
    }
}
