﻿using AutoMapper;
using Azure.Core;
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

                var employee = _dbContext.OrganizationEmployees
                    .Where(e => e.AppUserId == request.AppUserId && e.DismissalDate == null)
                    .FirstOrDefault();

                if (employee != null)
                {
                    return "This user is already your employee";
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
                
                if (res > 0)
                {
                    AddHistoryEvent(HistoryEventFactory.CreateAcceptRequestMasterEvent(request.AppUserId, organization.Name));
                    AddHistoryEvent(HistoryEventFactory.CreateAcceptRequestEvent(organization.AppUserId, user.FirstName + user.LastName));

                }
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
                var res = _dbContext.SaveChanges();

                if (res > 0)
                {
                    AddHistoryEvent(HistoryEventFactory.CreateRejectRequestMasterEvent(request.AppUserId, organization.Name));
                    AddHistoryEvent(HistoryEventFactory.CreateRejectRequestEvent(user.Id, user.FirstName + user.LastName));
                }

                return res.ToString();
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
                    var organization = _dbContext.Organizations.FirstOrDefault(o => o.AppUserId == user.Id);
                    if (organization == null)
                    {
                        var requests = _dbContext.RequestsToOrganization
                            .Where(r => r.AppUserId == user.Id)
                            .Include(x => x.AppUser)
                            .Include(x => x.Organization)
                            .ToList();
                        res = _mapper.Map<List<GetRequestDto>>(requests);
                    }
                    else
                    {
                        var requests = _dbContext.RequestsToOrganization
                            .Where(r => r.OrganizationId == organization.Id)
                            .Include(x => x.AppUser)
                            .Include(x => x.Organization)
                            .ToList();
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
                            var res = _dbContext.SaveChanges();

                            if (res > 0)
                            {
                                AddHistoryEvent(HistoryEventFactory.CreateOrgScheduleUpdateEvent(masterId));
                            }

                            return res.ToString();
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
                            var res = _dbContext.SaveChanges();

                            if (res > 0)
                            {
                                AddHistoryEvent(HistoryEventFactory.CreateOrgScheduleUpdateEvent(masterId));
                            }

                            return res.ToString();
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
                        if (info.Photo != null)
                        {
                            organization.Photo = info.Photo;
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
                            Photo = info.Photo,
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
                    res.Data = getOrgInfoByOwner(user.Id);
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

        private OrganizationInfo getOrgInfoByOwner(string orgIdOwner)
        {
            var organization = _dbContext.Organizations.FirstOrDefault(o => o.AppUserId == orgIdOwner);
            if (organization != null)
            {
                var res = new OrganizationInfo
                {
                    Id = organization.Id,
                    Name = organization.Name,
                    AppUserId = organization.AppUserId,
                    Photo = organization.Photo,
                    Description = organization.Description
                };
                return res;
            }
            return new OrganizationInfo();
        }


        public async Task<Result<OrganizationDetailsDto>> GetOrganizationDetails(int organizationId)
        {
            var res = new Result<OrganizationDetailsDto>();
            try
            {
                var user = await GetCurrentUserAsync();

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
                        AppUserId = organization.AppUserId,
                        Photo = organization.Photo,
                        Description = organization.Description
                    };

                    if (user != null)
                    {
                        var employee = _dbContext.OrganizationEmployees
                            .Where(e => e.AppUserId == user.Id && e.DismissalDate == null)
                            .FirstOrDefault();
                        if (employee != null)
                        {
                            data.IsInOrg = true;
                        }
                    }

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
                var employee = _dbContext.OrganizationEmployees.Where(e => e.Id == employeeId)
                    .Include(e => e.Organization)
                    .Include(e => e.AppUser)
                    .FirstOrDefault();
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
                    var res = _dbContext.SaveChanges();

                    if (res > 0)
                    {
                        AddHistoryEvent(HistoryEventFactory.CreateDismissEmployeeEvent(employee.AppUserId,employee.Organization.Name));
                        AddHistoryEvent(HistoryEventFactory.CreateDismissEmployeeOrgEvent(employee.Organization.AppUserId, employee.AppUser.FirstName + employee.AppUser.LastName));

                    }

                    return res.ToString();
                }
                return "Error during getting employee";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Result<List<OrganizationPortfolioDto>> GetOrganizationPortfolio(int orgId)
        {
            var res = new Result<List<OrganizationPortfolioDto>>();
            try
            {
                var organization = _dbContext.Organizations
                    .Where(o => o.Id == orgId)
                    .Include(o => o.Portfolios)
                    .FirstOrDefault();
                if (organization != null)
                {
                    var portfoliosIds = organization.Portfolios.Select(p => p.PortfolioId).ToList();
                    if (portfoliosIds != null)
                    {
                        var portfolios = _dbContext.Portfolios
                            .Where(p => portfoliosIds.Contains(p.Id))
                            .Include(p => p.AppUser)
                            .ToList();
                        if (portfolios != null)
                        {
                            res.Data = _mapper.Map<List<OrganizationPortfolioDto>>(portfolios);
                        }
                        else
                        {
                            res.Data= new List<OrganizationPortfolioDto>();
                        }
                    }
                }
                else
                {
                    res.ErrorMessage = "Error during finding organization";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
            }

            return res;
        }

        public Result<List<OrganizationPortfolioDto>> GetOrgMastersPortfolios(int orgId)
        {
            var res = new Result<List<OrganizationPortfolioDto>>();
            try
            {
                var organization = _dbContext.Organizations
                    .Where(o => o.Id == orgId)
                    .Include(o => o.OrganizationEmployees)
                    .FirstOrDefault();
                if (organization != null)
                {
                    var employeesIds = organization.OrganizationEmployees
                        .Where(e => e.DismissalDate == null).Select(o => o.AppUserId).ToList();
                    if(employeesIds != null)
                    {
                        var portfolios = _dbContext.Portfolios
                            .Where(p => employeesIds.Contains(p.AppUserId))
                            .Include(p => p.AppUser)
                            .ToList();
                        if (portfolios != null)
                        {
                            res.Data = _mapper.Map<List<OrganizationPortfolioDto>>(portfolios);
                        }
                        else
                        {
                            res.Data = new List<OrganizationPortfolioDto>();
                        }
                    }
                }
                else
                {
                    res.ErrorMessage = "Error during finding organization";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
            }

            return res;
        }

        public async Task<string> DeleteOrgPortfolio(int portfolioId)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var organization = _dbContext.Organizations.FirstOrDefault(o => o.AppUserId == user.Id);
                    if (organization == null) {
                        return "Your organization was not found";
                    }
                    var orgPortfolio = _dbContext.OrganizationPorfolios.FirstOrDefault(p => p.PortfolioId == portfolioId);
                    if (orgPortfolio != null)
                    {
                        if(orgPortfolio.OrganizationId == organization.Id)
                        {
                            _dbContext.Remove(orgPortfolio);
                            return _dbContext.SaveChanges().ToString();
                        }
                        
                            return "You don't have acces to this action";
                    }
                        return "Portfolio was not found";
                }
                    return "User was not found";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> AddOrgPortfolio(List<int> portfolioIds)
        {
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var organization = _dbContext.Organizations.Where(o => o.AppUserId == user.Id)
                        .Include(o => o.OrganizationEmployees)
                        .FirstOrDefault();
                    if (organization == null)
                    {
                        return "Your organization was not found";
                    }
                    var existingPortfolios = _dbContext.OrganizationPorfolios
                        .Where(o => portfolioIds.Contains(o.PortfolioId))
                        .Select(o => o.PortfolioId)
                        .ToList();
                    if(existingPortfolios != null)
                    {
                        portfolioIds = portfolioIds.Except(existingPortfolios).ToList();
                    }
                    var portfoliosToAdd = _dbContext.Portfolios.Where(p => portfolioIds.Contains(p.Id)).ToList();
                    if (portfoliosToAdd != null)
                    {
                        var users = portfoliosToAdd.Select(p => p.AppUserId).Distinct().ToList();
                        var employees = organization.OrganizationEmployees
                            .Where(e => e.DismissalDate == null)
                            .Select(e => e.AppUserId)
                            .Distinct().ToList();

                        if(users != null && employees != null && users.All(item => employees.Contains(item)))
                        {
                            var items = new List<OrganizationPorfolio>();
                            foreach (var p in portfoliosToAdd)
                            {
                                items.Add(new OrganizationPorfolio
                                {
                                    OrganizationId = organization.Id,
                                    PortfolioId = p.Id,
                                });
                            }
                            _dbContext.AddRange(items);
                            var res = _dbContext.SaveChanges();

                            if (res > 0)
                            {
                                var appUsers = _dbContext.Users.Where(u => users.Contains(u.Id)).ToList();
                                foreach (var employee in appUsers)
                                {
                                    AddHistoryEvent(HistoryEventFactory.CreateOrgAddPortfolioEvent(employee.Id, organization.Name));
                                    AddHistoryEvent(HistoryEventFactory.CreateOrgAddPortfolioOrgEvent(organization.AppUserId, employee.FirstName + employee.LastName));
                                }
                            }
                            else
                            {
                                return "No data were added";
                            }

                            return res.ToString();
                        }
                        return "You are trying to add portfolio of non-employee master";
                    }
                    return "Portfolios was not found";
                }
                return "User was not found";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<Result<List<ShortEmployeeDto>>> GetEmployeesNames()
        {
            var res = new Result<List<ShortEmployeeDto>>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var organization = _dbContext.Organizations
                        .Where(o => o.AppUserId == user.Id)
                        .Include(o => o.OrganizationEmployees)
                        .ThenInclude(e => e.AppUser)
                        .FirstOrDefault();
                    if (organization != null)
                    {
                        var employees = organization.OrganizationEmployees.Where(e => e.DismissalDate == null).Distinct();
                        res.Data = _mapper.Map<List<ShortEmployeeDto>>(employees);
                        return res;
                    }
                    res.ErrorMessage = "There is no such organization";
                }
                res.ErrorMessage = "Cannot find user";
            }
            catch(Exception ex) 
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<List<GetServiceDto>>> GetOrgServices(int? orgId)
        {
            var res = new Result<List<GetServiceDto>>();
            try
            {
                if (orgId == null)
                {
                    var user = await GetCurrentUserAsync();
                    if (user != null)
                    {
                        var organization = _dbContext.Organizations.FirstOrDefault(o => o.AppUserId == user.Id);
                        if (organization != null)
                        {
                            orgId = organization.Id;
                        }
                        else
                        {
                            res.ErrorMessage = "No organization was found";
                            return res;
                        }
                    }
                }

                var masters = _dbContext.OrganizationEmployees
                    .Where(e => e.OrganizationId == orgId && e.DismissalDate == null)
                    .Distinct()
                    .Select(m => m.AppUserId)
                    .ToList();
                if (masters != null && masters.Count > 0)
                {
                    var services = _dbContext.Services
                        .Where(s => masters.Contains(s.AppUserId))
                        .Include(s => s.AppUser)
                        .Include(s => s.Reviews)
                        .ToList();
                    if(services != null)
                    {
                        res.Data = _mapper.Map<List<GetServiceDto>>(services);
                        foreach (var service in res.Data)
                        {
                            if (service.Reviews.Count > 0)
                            {
                                service.Rate = service.Reviews.Average(r => r.Rate);
                                service.Reviews.Clear();
                            }
                        }
                    }
                    else
                    {
                        res.ErrorMessage = "No services were found";
                    }
                }
                else
                {
                    res.ErrorMessage = "No workers in this organization";
                }

            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
            }

            return res;
        }

        public async Task<Result<OrganizationInfo>> GetCurrentMasterOrg()
        {
            var res = new Result<OrganizationInfo>();
            try
            {
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var employee = _dbContext.OrganizationEmployees
                        .Where(s => s.AppUserId == user.Id && s.DismissalDate == null)
                        .Include(s => s.Organization)
                        .FirstOrDefault();
                    
                    if (employee != null && employee.Organization != null)
                    {
                        res.Data = getOrgInfoByOwner(employee.Organization.AppUserId);
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
    }
}
