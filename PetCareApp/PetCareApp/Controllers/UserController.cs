using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;
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

        [HttpGet("getUserPets")]
        public async Task<IActionResult> GetUserPets()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var res = await _userService.GetPets();
            if (res == null || res.Count == 0)
            {
                return NotFound();
            }

            return Ok(res);
        }

        [HttpGet("getPetById")]
        public IActionResult GetPetById([FromQuery]int petId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var res =  _userService.GetPetById(petId);
            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }

        [HttpGet("getMasterReviews")]
        [AllowAnonymous]
        public  async Task<IActionResult> GetMasterReviews([FromQuery] string masterId = "")
        {
            
            var res =  await _userService.GetMasterReviews(masterId);
            if (res.Data == null || res.Data.Count == 0)
            {
                return NotFound();
            }

            return Ok(res);
        }

        [HttpGet("getUserRecords")]
        public async Task<IActionResult> GetUserRecords()
        {
            var data = await _userService.GetUserRecords();
            if(data == null || data.Count == 0)
            {
                return NotFound("No records were found");
            }

            return Ok(data);
        }

        [HttpPatch("userCancelRecord")]
        public async Task<IActionResult> CancelRecord([FromQuery] int recordId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _userService.CancelRecord(recordId);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.ErrorMessage);
        }

        [HttpPost("addReview")]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _userService.AddReview(review);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.ErrorMessage);
        }

        [HttpPost("addReviewComment")]
        [Authorize(Roles = "Master,Organization")]
        public async Task<IActionResult> AddReviewComment([FromBody] ReviewCommentDto comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _userService.AddReviewComment(comment);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.ErrorMessage);
        }

        [HttpDelete("deleteReview")]
        public async Task<IActionResult> RemoveReview([FromQuery] int reviewId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _userService.RemoveReview(reviewId);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.ErrorMessage);
        }

        [HttpDelete("deleteReviewComment")]
        public async Task<IActionResult> RemoveReviewComment([FromQuery] int commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _userService.RemoveReviewComment(commentId);
            if (res.IsSuccess)
            {
                return Ok();
            }
            return BadRequest(res.ErrorMessage);
        }

        [HttpGet("getEventsHistory")]
        public async Task<IActionResult> GetEventsHistory()
        {
            var data = await _userService.GetEventsHistory();
            if (data == null)
            {
                return NotFound("No records were found");
            }

            return Ok(data);
        }
    }
}
