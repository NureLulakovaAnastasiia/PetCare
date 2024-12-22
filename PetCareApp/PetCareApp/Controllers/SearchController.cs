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
        private int maxCollectionCount = 10;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost("filters")]
        public async Task<IActionResult> FilterData(FiltersModel filters, [FromQuery]bool all=false)
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

            if (!all)
            {
                return Ok(res.GetRange(0, res.Count > maxCollectionCount ? maxCollectionCount : res.Count));
            }
            return Ok(res);
        }

        [HttpGet("AddCountries")]
        public async Task<IActionResult> AddCountries([FromQuery] string localization, string countryCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var res = await _searchService.AddCities(countryCode, localization);
            return Ok();
        }

        [HttpGet("getCitiesList")]
        public async Task<IActionResult> getCitiesList([FromQuery]int countryId, string localization = "en")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = _searchService.GetCitiesList(countryId, localization);
            if(res != null || res.Count > 0)
            {
                return Ok(res);
            }
            return NotFound();

        }
    }
}
