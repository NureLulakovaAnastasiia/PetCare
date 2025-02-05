using Microsoft.AspNetCore.Mvc;
using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface IOrganizationService
    {
        public Task<string> AcceptMasterRequest(int requestId);
        public Task<string> RejectMasterRequest(int requestId);
        public Task<List<GetRequestDto>> GetRequests();

        public Task<string> UpsertMasterSchedule(List<ScheduleDto> scheduleDto, string masterId);
        public Task<string> UpsertMasterBreaks(List<BreakDto> breakDto, string masterId);
        public Task<Result<OrganizationInfo>> GetOrganizationInfo(); 
        public Task<string> UpdateOrgInfo(OrganizationInfo info);
        public Result<OrganizationDetailsDto> GetOrganizationDetails(int organizationId);
        public Result<List<GetEmployeeDto>> GetOrgEmployees(int orgId);
        public string DismissEmployee(int employeeId);
        public Result<List<OrganizationPortfolioDto>> GetOrganizationPortfolio(int orgId);
        public Result<List<OrganizationPortfolioDto>> GetOrgMastersPortfolios(int orgId);

        public Task<string> DeleteOrgPortfolio(int portfolioId);
        public Task<string> AddOrgPortfolio(List<int> portfolioIds);
        public Task<Result<List<ShortEmployeeDto>>> GetEmployeesNames();
        public Task<Result<List<GetServiceDto>>> GetOrgServices(int? orgId);
    }


}
