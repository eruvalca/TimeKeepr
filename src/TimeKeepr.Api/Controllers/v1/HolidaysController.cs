using Microsoft.AspNetCore.Mvc;
using TimeKeepr.Application.Common.Dtos;
using TimeKeepr.Application.Holidays;
using TimeKeepr.Application.Holidays.Dtos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeKeepr.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HolidaysController : ControllerBase
    {
        private readonly HolidaysService _holidaysService;

        public HolidaysController(HolidaysService holidaysService)
        {
            _holidaysService = holidaysService;
        }

        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetByYear(int year)
        {
            var holidays = await _holidaysService.GetHolidaysByYearAsync(year);
            var response = RequestResult<IEnumerable<HolidayDto>>.Success(holidays);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var holiday = await _holidaysService.GetHolidayByIdAsync(id);

            if (holiday is not null)
            {
                var response = RequestResult<HolidayDto>.Success(holiday);
                return Ok(response);
            }
            else
            {
                var response = RequestResult<HolidayDto>.Failure(new string[] { "The holiday with the given id was not found." });
                return NotFound(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHolidayDto holidayDto)
        {
            var result = await _holidaysService.CreateHolidayAsync(holidayDto);

            if (result > 0)
            {
                var newHoliday = await _holidaysService.GetHolidayByIdAsync(result);

                if (newHoliday is not null)
                {
                    var successResponse = RequestResult<HolidayDto>.Success(newHoliday);
                    return CreatedAtAction("GetById", new { id = result }, successResponse);
                }

                var response = RequestResult<HolidayDto>.Failure(new string[] { "There was an issue creating the holiday." });
                return BadRequest(response);
            }
            else
            {
                var response = RequestResult<HolidayDto>.Failure(new string[] { "There was an issue creating the holiday." });
                return BadRequest(response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHolidayDto holidayDto)
        {
            if (id != holidayDto.HolidayId)
            {
                return BadRequest();
            }

            var result = await _holidaysService.UpdateHolidayAsync(holidayDto);

            if (result > 0)
            {
                return NoContent();
            }
            else if (result == 0)
            {
                var response = RequestResult<int>.Failure(new string[] { "There was an issue updating the holiday." });
                return BadRequest(response);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _holidaysService.DeleteHolidayAsync(id);

            if (result > 0)
            {
                return NoContent();
            }
            else
            {
                var response = RequestResult<int>.Failure(new string[] { "There was an issue deleting the holiday." });
                return BadRequest(response);

            }
        }
    }
}
