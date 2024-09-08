using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MasterController : ControllerBase
    {
        private readonly IMasterService _masterService;
        public MasterController(IMasterService masterService)
        {
            _masterService = masterService;
        }

        [HttpPost("addContacts")]
        public async Task<IActionResult> AddContacts(AddContactsDto contacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             
            var res = await _masterService.AddContacts(contacts);
            if (int.TryParse(res, out int num))
            {
                return Ok("Contacts was successfully added!");
            }

            return StatusCode(500, res);
        }


        [HttpPut("updateContacts")]
        public async Task<IActionResult> UpdateContacts(ContactsDto contacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.UpdateContacts(contacts);
            if (int.TryParse(res, out int num))
            {
                return Ok("Contacts was successfully updated!");
            }

            return StatusCode(500, res);
        }

        [HttpPost("addService")]
        [Authorize(Roles ="Master")]
        public async Task<IActionResult> AddService(AddServiceDto service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.AddService(service);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpPost("addQuestionary")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> AddQuestionary(List<AddQuestionDto> questionaryDto, [FromQuery] int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.AddQuestionary(questionaryDto, serviceId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpPut("updateQuestionary")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> UpdateQuestionary(List<UpdateQuestionDto> questionaryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.UpdateQuestionary(questionaryDto);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpPost("upsertSchedule")]
        public async Task<IActionResult> UpsertSchedule(List<ScheduleDto> scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.UpsertSchedule(scheduleDto);
            if (int.TryParse(res, out int num))
            {
                return Ok("Schedule was successfully added or updated!");
            }

            return StatusCode(500, res);
        }

        [HttpPost("upsertBreaks")]
        public async Task<IActionResult> UpsertBreaks(List<BreakDto> breakDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.UpsertBreaks(breakDto);
            if (int.TryParse(res, out int num))
            {
                return Ok("Breaks were successfully added or updated!");
            }

            return StatusCode(500, res);
        }

        [HttpGet("GetFreeTimeSlots")]
        public List<TimeSlot> GetTimeSlots(string masterId, int time)
        {
            var workSlots = _masterService.GetFreeTimeSlots(masterId, time);
            return workSlots;
        }

        [HttpPost("MakeAnAppointment")]
        public async Task<IActionResult> MakeAnAppointment(RecordDto record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _masterService.MakeAppointment(record);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }
    }
}
