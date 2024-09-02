using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;

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
        
    }
}
