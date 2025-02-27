using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Index.HPRtree;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Mappings;
using PetCareApp.Models;
using System.ComponentModel;

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

        private void AddHistoryEvent(HistoryEvent historyEvent)
        {
            try
            {
                _dbContext.Add(historyEvent);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
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
                    var result = await _dbContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateNewServiceEvent(user.Id, service.Name));
                    }
                   
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
            var res = "";
            try
            {
                var service = _mapper.Map<Models.Service>(serviceDto);
                var updatedTags = service.Tags != null ? service.Tags.ToList() : null;
                service.Tags = null;
                if (service != null)
                {
                    _dbContext.Update(service);
                    res = _dbContext.SaveChanges().ToString();
                    if (updatedTags != null)
                    {
                        UpdateServiceTags(service.Id, updatedTags);
                    }
                    return res;
                }
                return "Error during updating";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void UpdateServiceTags(int serviceId, List<Tag> tags)
        {
            var service = _dbContext.Services
                .Include(s => s.Tags)
                .FirstOrDefault(s => s.Id == serviceId);

            if (service == null || service.Tags == null)
            {
                throw new Exception("Service not found");
            }

            var tagsToRemove = service.Tags.Except(tags);
            var tagsToAdd = tags.Except(service.Tags);
            if (tagsToRemove != null)
            {
                service.Tags.RemoveAll(t => tagsToRemove.Contains(t));
            }
            if (tagsToAdd != null)
            {
                service.Tags.AddRange(tagsToAdd);
            }

            _dbContext.SaveChanges();
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

        public async Task<Result<GetServiceDto>> GetServiceDetails(int serviceId)
        {
            var res = new Result<GetServiceDto>();
            try
            {
                var service = _dbContext.Services
                        .Where(x => x.Id == serviceId)
                        .Include(x => x.Tags)
                        .Include (x => x.Reviews)
                        .FirstOrDefault();
               
                if (service != null)
                {
                    var serviceDto = _mapper.Map<GetServiceDto>(service);
                    if (serviceDto.Reviews.Count > 0)
                    {
                        serviceDto.Rate = service.Reviews.Average(r => r.Rate);
                        serviceDto.Reviews.Clear();
                    }
                    
                    var user = await GetCurrentUserAsync();
                    if (user != null && user.Id == serviceDto.AppUserId)
                    {
                        res.ErrorMessage = "Owner";
                    }
                    res.Data = serviceDto;
                }
                
            }
            catch(Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        public async Task<string> DeleteService(int serviceId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }

                var service = _dbContext.Services.FirstOrDefault(x => x.Id == serviceId);
                if (service != null && service.AppUserId == user.Id)
                {
                    _dbContext.Remove(service);
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateDeleteServiceEvent(user.Id, service.Name));
                    }
                    return result.ToString();
                }
                return "Error during adding";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> ChangeServiceVisibility(int serviceId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User not found";
                }

                var service = _dbContext.Services.FirstOrDefault(x => x.Id == serviceId);
                if (service != null && service.AppUserId == user.Id)
                {
                    service.IsHidden = !service.IsHidden;
                    _dbContext.Update(service);
                    var result = _dbContext.SaveChanges();
                    if (result > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateChangeServiceVisibilityEvent(user.Id, service.Name, service.IsHidden));
                    }
                    return result.ToString();
                }
                return "Error during adding";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public async Task<string> GetMasterName(int serviceId)
        {
            if(serviceId == 0)
            {
                return String.Empty;
            }

            var masterId = _dbContext.Services.Where(s => s.Id == serviceId).Select(s => s.AppUserId).FirstOrDefault();
            if(masterId != null)
            {
                var res = await _userManager.FindByIdAsync(masterId);
                if(res != null)
                {
                    return res.FirstName + " " + res.LastName;
                }
            }

            return String.Empty;
        }

        public async Task<Result<List<ReviewDto>>> GetServiceReviews(int serviceId)
        {
            var res = new Result<List<ReviewDto>>();

            try
            {
                var user = await GetCurrentUserAsync();
                var reviews = _dbContext.Reviews.Where(x => x.ServiceId == serviceId)
                       .Include(r => r.Comments)
                       .ThenInclude(c => c.AppUser)
                       .Include(r => r.AppUser)
                       .ToList();

                if (reviews != null)
                {
                    res.Data = new List<ReviewDto>();
                    foreach (var review in reviews)
                    {
                        var mapped = ReviewMapping.MapReview(review);
                        if (user != null)
                        {
                            if (mapped.AppUserId == user.Id)
                            {
                                mapped.isYours = true;
                            }
                            foreach (var comment in mapped.Comments)
                            {
                                if (comment.AppUserId == user.Id)
                                {
                                    comment.isYours = true;
                                }
                            }
                        }
                        res.Data.Add(mapped);
                    }
                }

                if (user != null)
                {
                    var service = _dbContext.Services.FirstOrDefault(s => s.Id == serviceId);
                    if (service != null && service.AppUserId == user.Id)
                    {
                        res.ErrorMessage = "Owner";
                    }
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }
            return res;
        }
    }
}
