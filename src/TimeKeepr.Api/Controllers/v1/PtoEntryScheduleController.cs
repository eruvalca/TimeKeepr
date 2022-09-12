using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeKeepr.Application.Common.Interfaces;
using TimeKeepr.Application.PtoEntries;
using TimeKeepr.Application.PtoEntries.Dtos;

namespace TimeKeepr.Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PtoEntryScheduleController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly PtoEntriesService _ptoEntriesService;

        public PtoEntryScheduleController(IIdentityService identityService, PtoEntriesService ptoEntriesService)
        {
            _identityService = identityService;
            _ptoEntriesService = ptoEntriesService;
        }

        [HttpGet]
        public async Task<IActionResult> AddYearlyVacationCarryOver()
        {
            var todayDate = DateTime.UtcNow.Date;

            if (todayDate.Month == 1 && todayDate.Day == 1)
            {
                var allUsersRequest = await _identityService.GetAllUsersAsync();

                if (allUsersRequest.Item1)
                {
                    var result = await _ptoEntriesService.AddYearlyVacationCarryOverAsync(allUsersRequest.Item2, todayDate);
                    
                    if (result)
                    {
                        return Ok();
                    }

                    return BadRequest("There was an issue generating and creating the yearly vacation carry over.");
                }
            }

            return NotFound();
        }
    }
}
