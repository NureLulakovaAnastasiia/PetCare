using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Interfaces;

namespace PetCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Organization,Admin")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost("acceptRequest")]
        public IActionResult AcceptRequest(int requestId)
        {
            var res = _organizationService.AcceptMasterRequest(requestId);
            if(res == "Success")
            {
                return Ok(res);
            }
            return StatusCode(500, res);
        }

        [HttpPost("rejectRequest")]
        public IActionResult RejectRequest(int requestId)
        {
            var res = _organizationService.RejectMasterRequest(requestId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }

        [HttpGet("getNewRequest")]
        public async Task<IActionResult> GetNewRequests()
        {
            var res = await _organizationService.GetNewRequests();
            if(res != null)
            {
                if(res.Count > 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            return StatusCode(500);
        }

    }
}
