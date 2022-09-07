using Microsoft.AspNetCore.Mvc;
using TimeKeepr.Application.Common.Dtos;
using TimeKeepr.Application.PtoEntries;
using TimeKeepr.Application.PtoEntries.Dtos;

namespace TimeKeepr.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PtoEntriesController : ControllerBase
    {
        private readonly PtoEntriesService _ptoEntriesService;

        public PtoEntriesController(PtoEntriesService ptoEntriesService)
        {
            _ptoEntriesService = ptoEntriesService;
        }

        [HttpGet("{applicationUserId}/{year}")]
        public async Task<IActionResult> GetByUserAndYear(string applicationUserId, int year)
        {
            var ptoEntries = await _ptoEntriesService.GetUserPtoEntriesByYearAsync(applicationUserId, year);
            var response = RequestResult<IEnumerable<PtoEntryDto>>.Success(ptoEntries);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ptoEntry = await _ptoEntriesService.GetPtoEntryByIdAsync(id);

            if (ptoEntry is not null)
            {
                var response = RequestResult<PtoEntryDto>.Success(ptoEntry);
                return Ok(response);
            }
            else
            {
                var response = RequestResult<PtoEntryDto>.Failure(new string[] { "The entry with the given id was not found." });
                return NotFound(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePtoEntryDto ptoEntryDto)
        {
            var result = await _ptoEntriesService.CreatePtoEntryAsync(ptoEntryDto);

            if (result > 0)
            {
                var newPtoEntry = await _ptoEntriesService.GetPtoEntryByIdAsync(result);

                if (newPtoEntry is not null)
                {
                    var successResponse = RequestResult<PtoEntryDto>.Success(newPtoEntry);
                    return CreatedAtAction("GetById", new { id = result }, successResponse);
                }

                var response = RequestResult<PtoEntryDto>.Failure(new string[] { "There was an issue creating the PTO entry." });
                return BadRequest(response);
            }
            else
            {
                var response = RequestResult<PtoEntryDto>.Failure(new string[] { "There was an issue creating the PTO entry." });
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePtoEntryDto ptoEntryDto)
        {
            if (id != ptoEntryDto.PtoEntryId)
            {
                var response = RequestResult<int>.Failure(new string[] { "There was an issue updating the PTO entry." });
                return BadRequest(response);
            }

            var result = await _ptoEntriesService.UpdatePtoEntryAsync(ptoEntryDto);

            if (result > 0)
            {
                return NoContent();
            }
            else if (result == 0)
            {
                var response = RequestResult<int>.Failure(new string[] { "There was an issue updating the PTO entry." });
                return BadRequest(response);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ptoEntriesService.DeletePtoEntryAsync(id);

            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                var response = RequestResult<int>.Failure(new string[] { "There was an issue deleting the PTO entry." });
                return BadRequest(response);
            }
        }
    }
}
