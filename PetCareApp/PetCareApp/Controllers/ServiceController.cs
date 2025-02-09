using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Services;

namespace PetCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceApiService _serviceService;
        public ServiceController(IServiceApiService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpPost("addService")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> AddService([FromBody]AddServiceDto service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _serviceService.AddService(service);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpPatch("updateService")]
        [Authorize(Roles = "Master")]
        public IActionResult UpdateService(UpdateServiceDto service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _serviceService.UpdateService(service);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        //if master requests - no parameter, if user about master - parameter is needed
        [HttpGet("getMasterServices")] 
        public async Task<IActionResult> GetMasterServices(string? masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var services = await _serviceService.GetServices(masterId);
            if (services == null || services.Count == 0)
            {
                return NotFound();
            }
            return Ok(services);
        }

        [HttpGet("getServiceDetails")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetServiceDetails(int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _serviceService.GetServiceDetails(serviceId);
            if (res == null)
            {
                return NotFound();
            }else if(res.IsSuccess || res.ErrorMessage == "Owner")
            {
                return Ok(res);
            }

            return StatusCode(500, res.ErrorMessage);
        }

        [HttpDelete("deleteService")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> DeleteService([FromQuery] int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _serviceService.DeleteService(serviceId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpGet("changeServiceVisibility")]
        [Authorize(Roles = "Master")]
        public async Task<IActionResult> ChangeServiceVisibility([FromQuery] int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _serviceService.ChangeServiceVisibility(serviceId);
            if (int.TryParse(res, out int num))
            {
                return Ok(num);
            }

            return StatusCode(500, res);
        }

        [HttpGet("getMasterServicesNames")]
        public async Task<IActionResult> GetMasterServicesNames(string? masterId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var services = await _serviceService.GetServices(masterId);
            if (services == null || services.Count == 0)
            {
                return NotFound();
            }
            var dict = new Dictionary<int, string>();
            foreach (var service in services)
            {
                dict[service.Id] = service.Name;
            }
            return Ok(dict);
        }

        [HttpGet("getMasterName")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMasterName([FromQuery]int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var masterName = await _serviceService.GetMasterName(serviceId);
            if (String.IsNullOrEmpty(masterName))
            {
                return NotFound();
            }
            
            return Ok(masterName);
        }

        [HttpGet("getServiceReviews")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceReviews([FromQuery] int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviews = await _serviceService.GetServiceReviews(serviceId);
            if (reviews == null || reviews.Count == 0)
            {
                return NotFound();
            }

            return Ok(reviews);
        }
    }
}
