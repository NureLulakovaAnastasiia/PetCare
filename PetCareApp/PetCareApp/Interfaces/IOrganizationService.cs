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

    }


}
