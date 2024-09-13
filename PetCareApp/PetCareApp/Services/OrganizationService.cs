using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Models;

namespace PetCareApp.Services
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public OrganizationService(UserManager<AppUser> userManager, IMapper mapper,  IHttpContextAccessor httpContextAccessor,
            ApplicationDBContext applicationDBContext) : base(userManager, httpContextAccessor)
        {
            _dbContext = applicationDBContext;
            _mapper = mapper;
        }

        public string AcceptMasterRequest(int requestId)
        {
            try
            {
                var request = _dbContext.RequestsToOrganization.FirstOrDefault(x => x.Id == requestId);
                if (request == null)
                {
                    return "Request is not found";
                }
                request.Status = "Accepted";
                _dbContext.Update(request);
                _dbContext.Add(new OrganizationEmployee
                {
                    AppUserId = request.AppUserId,
                    OrganizationId = request.OrganizationId,
                    AcceptanceDate = DateTime.UtcNow
                });
                var res = _dbContext.SaveChanges();
                if (res != 2)
                {
                    return "Error during accepting";
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string RejectMasterRequest(int requestId)
        {
            try
            {
                var request = _dbContext.RequestsToOrganization.FirstOrDefault(x => x.Id == requestId);
                if (request == null)
                {
                    return "Request is not found";
                }
                request.Status = "Rejected";
                _dbContext.Update(request);
                return _dbContext.SaveChanges().ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<GetRequestDto>> GetNewRequests()
        {
            var res = new List<GetRequestDto>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var organization = _dbContext.Organizations.FirstOrDefault(x => x.AppUserId == user.Id);
                    if (organization != null)
                    {
                        var requests = _dbContext.RequestsToOrganization
                            .Where(x => x.OrganizationId == organization.Id && x.Status == "New").ToList();
                        res = _mapper.Map<List<GetRequestDto>>(requests);
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }
    }
}
