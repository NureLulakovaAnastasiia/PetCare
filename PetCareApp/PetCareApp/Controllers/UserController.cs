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

    }
}
