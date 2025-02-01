using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PetCareApp.Data;
using PetCareApp.Dtos;
using PetCareApp.Interfaces;
using PetCareApp.Mappings;
using PetCareApp.Models;

namespace PetCareApp.Services
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMapper _mapper;
        public OrganizationService(UserManager<AppUser> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ApplicationDBContext applicationDBContext) : base(userManager, httpContextAccessor)
        {
            _dbContext = applicationDBContext;
            _mapper = mapper;
        }

        public async Task<string> AcceptMasterRequest(int requestId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User was not found";
                }
                var organization  = _dbContext.Organizations.FirstOrDefault(x => x.AppUserId == user.Id);
                if (organization == null)
                {
                    return "You don't have access to this function";
                }

                var request = _dbContext.RequestsToOrganization.FirstOrDefault(x => x.Id == requestId);
                if (request == null)
                {
                    return "Request is not found";
                }else if(request.OrganizationId != organization.Id)
                {
                    return "You don't have access to this function";
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

        public async Task<string> RejectMasterRequest(int requestId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user == null)
                {
                    return "User was not found";
                }
                var organization = _dbContext.Organizations.FirstOrDefault(x => x.AppUserId == user.Id);
                if (organization == null)
                {
                    return "You don't have access to this function";
                }

                var request = _dbContext.RequestsToOrganization.FirstOrDefault(x => x.Id == requestId);
                if (request == null)
                {
                    return "Request is not found";
                }
                else if (request.OrganizationId != organization.Id)
                {
                    return "You don't have access to this function";
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

        public async Task<List<GetRequestDto>> GetRequests()
        {
            var res = new List<GetRequestDto>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                        var requests = _dbContext.RequestsToOrganization
                            .Include(x => x.AppUser)
                            .Include(x => x.Organization)
                            .ToList();
                        res = _mapper.Map<List<GetRequestDto>>(requests);
                }
                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }

        public async Task<string> UpsertMasterSchedule(List<ScheduleDto> scheduleDto, string masterId)
        {
            if (scheduleDto.Count == 0)
            {
                return "Nothing to add or update";
            }
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    if (IsInOrganization(masterId, user.Id))
                    {
                        var schedule = _mapper.Map<List<Schedule>>(scheduleDto);
                        if (schedule != null)
                        {
                            foreach (var item in schedule)
                            {
                                item.AppUserId = masterId;
                                if (item.Id == 0)
                                {
                                    _dbContext.Add(item);
                                }
                                else
                                {
                                    _dbContext.Update(item);
                                }
                            }
                            return _dbContext.SaveChanges().ToString();
                        }
                    }
                    return "This master is not in your organization";
                }

                return "Error while adding or updating schedule";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UpsertMasterBreaks(List<BreakDto> breakDto, string masterId)
        {
            if (breakDto.Count == 0)
            {
                return "Nothing to add or update";
            }
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    if (IsInOrganization(masterId, user.Id))
                    {
                        var breaks = _mapper.Map<List<Break>>(breakDto);
                        if (breaks != null)
                        {
                            foreach (var item in breaks)
                            {
                                item.AppUserId = masterId;
                                if (item.Id == 0)
                                {
                                    _dbContext.Add(item);
                                }
                                else
                                {
                                    _dbContext.Update(item);
                                }
                            }
                            return _dbContext.SaveChanges().ToString();
                        }
                    }
                    return "This master is not in your organization";
                }

                return "Error while adding or updating schedule";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private bool IsInOrganization(string masterId, string OrgAdminId)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == OrgAdminId);
            if (user != null)
            {
                var master = _dbContext.Users.FirstOrDefault(x => x.Id == masterId);
                var organization = _dbContext.Organizations.FirstOrDefault(x => x.AppUserId == user.Id);
                if (master != null && organization != null)
                {
                    var isInOrganization = _dbContext.OrganizationEmployees.Any(x => x.OrganizationId == organization.Id &&
                                                                                x.AppUserId == master.Id &&
                                                                                x.DismissalDate == null);
                    return isInOrganization;
                }
            }
            return false;
        }

        public async Task<string> UpdateOrgInfo(OrganizationInfo info)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null) { 
                    var organization = _dbContext.Organizations.FirstOrDefault(o => o.AppUserId == user.Id);
                    if (organization != null)
                    {
                        if(!String.IsNullOrEmpty(info.Name))
                        {
                            organization.Name = info.Name;
                        }

                        if (!String.IsNullOrEmpty(info.Description))
                        {
                            organization.Description = info.Description;
                        }

                        _dbContext.Organizations.Update(organization);
                        return _dbContext.SaveChanges().ToString();
                    }
                    else {
                        organization = new Organization
                        {
                            AppUserId = user.Id,
                            Name = info.Name,
                            Description = info.Description,
                            Id = 0
                        };
                        _dbContext.Organizations.Add(organization);
                        return _dbContext.SaveChanges().ToString();
                    }
                }
                else
                {
                    return "User was not found";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<Result<OrganizationInfo>> GetOrganizationInfo()
        {
            var res = new Result<OrganizationInfo>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var organization = _dbContext.Organizations.FirstOrDefault(o => o.AppUserId == user.Id);
                    if (organization != null)
                    {
                        res.Data = new OrganizationInfo
                        {
                            Id = organization.Id,
                            Name = organization.Name,
                            Description = organization.Description
                        };
                    }
                    else
                    {
                        res.Data = new OrganizationInfo();
                    }
                }
                else
                {
                    res.ErrorMessage = "User was not found";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public Result<OrganizationDetailsDto> GetOrganizationDetails(int organizationId)
        {
            var res = new Result<OrganizationDetailsDto>();
            try
            {
                var organization = _dbContext.Organizations.Where(o => o.Id == organizationId)
                   .Include(o => o.AppUser) 
                   .ThenInclude(u => u.Contacts)
                   .ThenInclude(c => c.Location)
                   .FirstOrDefault();

                if(organization != null)
                {
                    var data = new OrganizationDetailsDto
                    {
                        Id = organization.Id,
                        Name = organization.Name,
                        Description = organization.Description
                    };
                    if (organization.AppUser != null)
                    {
                        var contacts = ContactsMapping.MapContact(organization.AppUser.Contacts);
                        data.Contacts = _mapper.Map<GetContactsDto>(contacts);
                    }
                    else
                    {
                        data.Contacts = new GetContactsDto();
                    }

                    res.Data = data;
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public Result<List<GetEmployeeDto>> GetOrgEmployees(int orgId)
        {
            var res = new Result<List<GetEmployeeDto>>();
            try
            {
                var organization = _dbContext.Organizations.FirstOrDefault(o => o.Id == orgId);
                if (organization != null)
                {
                    var employees = _dbContext.OrganizationEmployees.Where(e => e.OrganizationId == orgId)
                        .Include(e => e.AppUser)
                        .Include(e => e.Organization)
                        .ToList();

                    if (employees != null)
                    {
                        res.Data = _mapper.Map<List<GetEmployeeDto>>(employees);
                    }
                    else
                    {
                        res.Data = new List<GetEmployeeDto>();
                    }
                }
                else
                {
                    res.ErrorMessage = "This organization was not found";
                }
            }catch(Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }
            return res;
        }

        public string DismissEmployee(int employeeId)
        {
            try
            {
                var employee = _dbContext.OrganizationEmployees.FirstOrDefault(e => e.Id == employeeId);
                if (employee != null)
                {
                    var portfolios = _dbContext.Portfolios.Where(p => p.AppUserId == employee.AppUserId)
                        .Select(p => p.Id).ToList();
                    if (portfolios != null)
                    {
                        var orgPortfolios = _dbContext.OrganizationPorfolios.Where(p => portfolios.Contains(p.PortfolioId)).ToList();
                        if (orgPortfolios != null)
                        {
                            _dbContext.RemoveRange(orgPortfolios);
                        }
                    }
                    employee.DismissalDate = DateTime.UtcNow;  
                    _dbContext.Update(employee);
                    return _dbContext.SaveChanges().ToString();
                }
                return "Error during getting employee";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
