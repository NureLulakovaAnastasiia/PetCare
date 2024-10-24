using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Services
{
    public class ServiceApiService : BaseService, IServiceApiService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _dbContext;
        public ServiceApiService(UserManager<AppUser> userManager, IMapper mapper, ApplicationDBContext context,
           IHttpContextAccessor httpContextAccessor) : base(userManager, httpContextAccessor)
        {
            _mapper = mapper;
            _dbContext = context;
        }

        public async Task<string> AddService(AddServiceDto serviceDto)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }

                var service = _mapper.Map<Models.Service>(serviceDto);
                if (service != null)
                {
                    service.AppUserId = user.Id;
                    _dbContext.Add(service);
                    await _dbContext.SaveChangesAsync();
                    return service.Id.ToString();
                }
                return "Error during adding";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UpdateService(UpdateServiceDto serviceDto)
        {
            try
            {
                var service = _mapper.Map<Models.Service>(serviceDto);
                if (service != null)
                {
                    var res = _dbContext.Update(service);
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during updating";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<GetServiceDto>> GetServices(string? masterId)
        {
            if (masterId == null)
            {
                var user = await GetCurrentUserAsync();
                if(user != null)
                {
                    masterId = user.Id;
                }
            }
            var res = new List<GetServiceDto>();
            try
            {
                var masterExist = _dbContext.Users.Any(x => x.Id == masterId);
                if (masterExist)
                {
                    var masterServices = _dbContext.Services
                         .Where(x => x.AppUserId == masterId && !x.IsHidden)
                         .Include(x => x.Reviews).ToList();

                    if (masterServices.Count > 0)
                    {
                        res = _mapper.Map<List<GetServiceDto>>(masterServices);
                    }
                }
                return res;
            }
            catch
            {
                return res;
            }
        }

        public GetServiceDto GetServiceDetails(int serviceId)
        {
            var res = new GetServiceDto();
            try
            {
                var service = _dbContext.Services
                        .Where(x => x.Id == serviceId)
                        .Include(x => x.Reviews).FirstOrDefault();

                if (service != null)
                {
                    res = _mapper.Map<GetServiceDto>(service);
                }

                return res;
            }
            catch
            {
                return res;
            }
        }
    }
}
