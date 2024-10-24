﻿using Microsoft.AspNetCore.Authorization;
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

        

        [HttpGet("getMasterGeneralData")]
        public async Task<IActionResult> GetMasterGeneralData()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _masterService.GetGeneralMasterData();
            if (data == null || String.IsNullOrEmpty(data.FirstName))
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPatch("updateMasterGeneralData")]
        public async Task<IActionResult> UpdateMasterGeneralData([FromBody] GetGeneralMasterDto masterData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _masterService.UpdateGeneralMasterData(masterData);
            if (!String.IsNullOrEmpty(data))
            {
                return StatusCode(500, data);
            }
            return Ok();
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
        [Authorize(Roles = "Master,Admin")]
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
        [Authorize(Roles = "Master,Admin")]
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

        [HttpPost("analizeQuestionary")]
        public IActionResult AnalizeQuestionaryGetSlots(List<QuestionDto> questionary, int serviceId, string masterId)
        {
            var descr = _masterService.GetQuestionaryDescription(questionary);
            var time = _masterService.AnalizeQuestionary(questionary, serviceId);
            if(time == 0)
            {
                return StatusCode(500, "Error during analizing questionary");
            }
            var slots = _masterService.GetFreeTimeSlots(masterId, time, serviceId);
            if (!slots.Any())
            {
                return StatusCode(500, "Error during getting timeslots");
            }
            

            return Ok(new QuestionaryAnalisysDto
            {
                Time = time,
                Slots = slots,
                Description = descr
            });
        }

        [HttpPost("makeRequest")]
        public async Task<IActionResult> MakeRequest(int organizationId)
        {
            var res = await _masterService.MakeRequestToOrganization(organizationId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpPost("CancelRecord")]
        public async Task<IActionResult> CancelRecord(int recordId, string reason)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _masterService.CancelRecord(recordId, reason);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }


    }
}
