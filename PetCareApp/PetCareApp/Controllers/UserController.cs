using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using System.Runtime.CompilerServices;

namespace PetCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("addPet")]
        public async Task<IActionResult> AddPet(PetDto pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _userService.AddPet(pet);
            if (res.IsSuccess)
            {
                return Ok(res.Data);
            }

            return StatusCode(500, res.ErrorMessage);
        }

        [HttpPut("updatePets")]
        public async Task<IActionResult> UpdatePets(List<PetDto> pets)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _userService.UpdatePets(pets);
            if (int.TryParse(res, out int num))
            {
                return Ok("Pets were successfully updated!");
            }

            return StatusCode(500, res);
        }

        [HttpDelete("deletePet")]
        public async Task<IActionResult> DeletePet([FromQuery]int petId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _userService.RemovePet(petId);
            if (int.TryParse(res, out int num))
            {
                return Ok("Pet was successfully deleted!");
            }

            return StatusCode(500, res);
        }

        [HttpGet("getMasterData")]
        [AllowAnonymous]
        public IActionResult GetMasterData([FromQuery]string masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var res = _userService.GetMasterData(masterId);
            if (String.IsNullOrEmpty(res.Id))
            {
                return NotFound();
            }

            return Ok(res);
        }

        [HttpGet("getMasterReviews")]
        [AllowAnonymous]
        public IActionResult GetMasterReviews([FromQuery] string masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var res = _userService.GetMasterReviews(masterId);
            if (res.Count == 0)
            {
                return NotFound();
            }

            return Ok(res);
        }
    }
}
