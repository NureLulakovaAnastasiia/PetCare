using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;
using PetCareApp.Services;

namespace PetCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Organization,Master")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpPost("addPorfolio")]
        public async Task<IActionResult> AddPorfolio(List<AddPortfolioDto> portfolioDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _portfolioService.AddPortfolio(portfolioDtos);
            if (int.TryParse(res, out int num))
            {
                return Ok("Portfolio was successfully added!");
            }

            return StatusCode(500, res);
        }

        [HttpPut("updatePortfolio")]
        public async Task<IActionResult> UpdatePortfolio(List<PortfolioDto> portfolioDtos)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _portfolioService.UpdatePortfolio(portfolioDtos);
            if (int.TryParse(res, out int num))
            {
                return Ok("Portfolio was successfully updated!");
            }

            return StatusCode(500, res);
        }

        [HttpDelete("deletePortfolio")]
        public async Task<IActionResult> DeletePortfolio(List<int> portfolioIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _portfolioService.RemovePortfolio(portfolioIds);
            if (int.TryParse(res, out int num))
            {
                return Ok("Portfolio was successfully deleted!");
            }

            return StatusCode(500, res);
        }

        [HttpGet("getPortfolio")]
        public IActionResult GetPortfolio(string masterId)
        {
            var res = _portfolioService.GetMasterPortfolio(masterId);
            if(res == null || res.Count == 0)
            {
                return NotFound("portfolio is empty or not exist");
            }
            return Ok(res);
        }
    }
}
