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
        public async Task<IActionResult> AddQuestionary([FromBody] List<AddQuestionDto> questionaryDto, [FromQuery] int serviceId)
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

        [HttpDelete("deleteQuestionary")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> DeleteQuestionary([FromQuery] int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.DeleteQuestionary(serviceId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpGet("getQuestionary")]
        public async Task<IActionResult> GetQuestionary([FromQuery] int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.GetQuestionary(serviceId);
            if (res == null || res.Count == 0)
            {
                return StatusCode(500, "Error during getting questionary");
            }

            return Ok(res);
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
        [Authorize(Roles = "Master,Organization,Admin")]
        public async Task<IActionResult> UpsertBreaks(List<BreakDto> breakDto, [FromQuery] string? masterId = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.UpsertBreaks(breakDto, masterId);
            if (int.TryParse(res, out int num))
            {
                return Ok("Breaks were successfully added or updated!");
            }

            return StatusCode(500, res);
        }

        [HttpPost("deleteBreaks")]
        [Authorize(Roles = "Master,Organization,Admin")]
        public async Task<IActionResult> DeleteBreaks(List<int> breaksIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.DeleteBreaks(breaksIds);
            if (int.TryParse(res, out int num))
            {
                return Ok("Breaks were successfully deleted!");
            }

            return StatusCode(500, res);
        }


        [HttpPost("makeAnAppointment")]
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

        [HttpPut("updateAppointments")]
        public async Task<IActionResult> UpdateAnAppointments(List<GetRecordDto> records)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _masterService.UpdateAppointments(records);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            if (res == "Unauthorized")
            {
                return StatusCode(401, res);
            }

            return StatusCode(500, res);
        }

        [HttpPost("analizeQuestionary")]
        public IActionResult AnalizeQuestionaryGetSlots(List<QuestionDto> questionary, [FromQuery]int serviceId, string? masterId = null)
        {
            var descr = _masterService.GetQuestionaryDescription(questionary);
            var time = _masterService.AnalizeQuestionary(questionary, serviceId);
            if (time == 0)
            {
                return StatusCode(500, "Error during analizing questionary");
            }
            var slots = _masterService.GetFreeTimeSlots(time, serviceId, masterId);
            var optimizedSlots  = _masterService.GetBetterFreeTimeSlots(time, serviceId, masterId);
            
            if (!slots.Any())
            {
                return StatusCode(500, "Error during getting timeslots");
            }


            return Ok(new QuestionaryAnalisysDto
            {
                Time = time,
                Slots = slots,
                OptimizedSlots = optimizedSlots,
                Description = descr
            });
        }

        [HttpPatch("makeRequest")]
        public async Task<IActionResult> MakeRequest([FromQuery]int organizationId)
        {
            var res = await _masterService.MakeRequestToOrganization(organizationId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpPost("CancelRecord")]
        public async Task<IActionResult> CancelRecord([FromQuery] int recordId, [FromBody] string reason)
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

        [HttpGet("getMasterRecordsForMonth")]
        public async Task<IActionResult> GetMasterRecordsForMonth(int month, int year, string? masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _masterService.GetRecordsForMonth(month, year, masterId);
            if (data.IsSuccess)
            {
                return Ok(data.Data);
            }
            return StatusCode(500, data.ErrorMessage);
        }

        [HttpGet("getMasterBreaks")]
        public async Task<IActionResult> GetMasterBreaks([FromQuery]string? masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _masterService.GetMasterBreaks(masterId);
            if (data.IsSuccess)
            {
                return Ok(data.Data);
            }
            return StatusCode(500, data.ErrorMessage);
        }


        [HttpGet("getPortfolio")] //if masterId is empty - search for portfolio of current user
        [AllowAnonymous]
        public async Task<IActionResult> GetMasterPortfolio(string? masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _masterService.GetMasterPortfolio(masterId);
            if (data.IsSuccess)
            {
                return Ok(data.Data);
            }
            return StatusCode(500, data.ErrorMessage);
        }

        [HttpPost("upsertPortfolio")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> UpsertPortfolio([FromBody] List<PortfolioDto> portfolioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _masterService.UpsertPortfolio(portfolioDto);
            if (int.TryParse(res.ErrorMessage, out int num))
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res);
        }

        [HttpDelete("deletePortfolio")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> DeletePortfolio([FromQuery] int porfolioId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _masterService.DeletePortfolio(porfolioId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }


        [HttpGet("getMasterServices")] //if masterId is empty - search for services of current user
        [AllowAnonymous]
        public async Task<IActionResult> GetMasterServices(string? masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _masterService.GetMasterServices(masterId);
            if (data.IsSuccess)
            {
                return Ok(data.Data);
            }
            return StatusCode(500, data.ErrorMessage);
        }

        [HttpGet("getUserQuestionary")] 
        public IActionResult GetUserQuestionary([FromQuery]int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _masterService.GetQuestionaryForUser(serviceId);
            if (data.IsSuccess)
            {
                return Ok(data.Data);
            }
            return StatusCode(500, data.ErrorMessage);
        }

        [HttpGet("getMasterSchedule")] //if masterId is empty - search for schedule of current user
        [AllowAnonymous]
        public async Task<IActionResult> GetMasterSchedule(string? masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = await _masterService.GetMasterSchedule(masterId);
            if (data.IsSuccess)
            {
                return Ok(data.Data);
            }
            return StatusCode(500, data.ErrorMessage);
        }

        [HttpPut("updateSchedule")]
        [Authorize(Roles = "Master,Organization")]
        public IActionResult UpdateSchedule([FromBody] List<ScheduleDto> scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res =  _masterService.UpdateMasterSchedule(scheduleDto);
            if (res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpDelete("deleteSchedule")]
        [Authorize(Roles = "Master,Organization")]
        public async Task<IActionResult> DeleteSchedule([FromQuery]int scheduleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _masterService.DeleteSchedule(scheduleId);
            if (res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpPost("addSchedule")]
        [Authorize(Roles = "Master,Organization")]
        public async Task<IActionResult> AddSchedule([FromBody] ScheduleDto scheduleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = await _masterService.AddMasterSchedule(scheduleDto);
            if (res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpGet("getRecordOwner")]
        [Authorize]
        public async Task<IActionResult> GetRecordOwner([FromQuery] int recordId)
        {
            var res = await _masterService.GetRecordOwner(recordId);
            
                return Ok(res);
            
        }
    }
}
