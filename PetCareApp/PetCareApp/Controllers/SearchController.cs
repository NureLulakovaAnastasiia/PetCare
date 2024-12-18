using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("search/filters")]
        public async Task<IActionResult> FilterData(FiltersModel filters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = _searchService.FindServices(filters);
            if (res == null || res.Count == 0)
            {
                return NotFound("No data was found");
            }

            return Ok(res);
        }

        [HttpGet("AddCountries")]
        public async Task<IActionResult> AddCountries([FromQuery] string localization, string countryCode)
        {
            if (ModelState.IsValid)
            {
            }
            var res = await _searchService.AddCities(countryCode, localization);
            return Ok();
        }
    }
}
