using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost("acceptRequest")]
        [Authorize(Roles = "Organization")]
        public async Task<IActionResult> AcceptRequest([FromQuery]int requestId)
        {
            var res =  await _organizationService.AcceptMasterRequest(requestId);
            if (res == "Success")
            {
                return Ok(res);
            }
            return StatusCode(500, res);
        }

        [HttpPost("rejectRequest")]
        [Authorize(Roles = "Organization")]

        public async Task<IActionResult> RejectRequest([FromQuery] int requestId)
        {
            var res = await _organizationService.RejectMasterRequest(requestId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }

        [HttpGet("getRequests")]
        [Authorize(Roles = "Master, Organization")]
        public async Task<IActionResult> GetRequests()
        {
            var res = await _organizationService.GetRequests();
            if (res != null)
            {
                if (res.Count > 0)
                {
                    return Ok(res);
                }
                return NotFound();
            }
            return StatusCode(500);
        }

        [HttpGet("getOrganizationData")]
        [Authorize(Roles = "Organization")]

        public async Task<IActionResult> GetOrgData()
        {
            var res = await _organizationService.GetOrganizationInfo();
            if (res != null && res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpPost("updateOrganizationInfo")]
        [Authorize(Roles = "Organization")]

        public async Task<IActionResult> UpdateOrgInfo(OrganizationInfo info)
        {
            var res = await _organizationService.UpdateOrgInfo(info);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }

        [HttpGet("getOrganizationDetails")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrganizationDetails([FromQuery]int organizationId)
        {
            var res = _organizationService.GetOrganizationDetails(organizationId);
            if (res != null && res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpGet("getOrganizationEmployees")]
        [AllowAnonymous]
        public IActionResult GetOrganizationEmployees([FromQuery] int orgId)
        {
            var res =  _organizationService.GetOrgEmployees(orgId);
            if (res != null && res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpPatch("dismissEmployee")]
        [Authorize(Roles = "Organization")]

        public IActionResult DismissEmployee([FromQuery] int employeeId)
        {
            var res = _organizationService.DismissEmployee(employeeId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }

        [HttpGet("getOrganizationPortfolio")]
        [AllowAnonymous]
        public IActionResult GetOrganizationPortfolio([FromQuery] int orgId)
        {
            var res = _organizationService.GetOrganizationPortfolio(orgId);
            if (res != null && res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpGet("getAllOrgMastersPortfolio")]
        [AllowAnonymous]
        public IActionResult getAllOrgMastersPortfolio([FromQuery] int orgId)
        {
            var res = _organizationService.GetOrgMastersPortfolios(orgId);
            if (res != null && res.IsSuccess)
            {
                return Ok(res.Data);
            }
            return StatusCode(500, res.ErrorMessage);
        }

        [HttpDelete("removeOrgPortfolio")]
        [Authorize(Roles = "Organization")]

        public async Task<IActionResult> RemoveOrgPortfolio([FromQuery] int portfolioId)
        {
            var res = await _organizationService.DeleteOrgPortfolio(portfolioId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }

        [HttpPost("addOrgPorfolios")]
        [Authorize(Roles = "Organization")]

        public async Task<IActionResult> AddOrgPortfolio([FromBody] List<int> portfoliosIds)
        {
            var res = await _organizationService.AddOrgPortfolio(portfoliosIds);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }
            return StatusCode(500, res);
        }
    }
}
