﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AddService(AddServiceDto service)
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
        public IActionResult GetServiceDetails(int serviceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var service =  _serviceService.GetServiceDetails(serviceId);
            if (service == null || service.Id == 0)
            {
                return NotFound();
            }
            return Ok(service);
        }
    }
}
