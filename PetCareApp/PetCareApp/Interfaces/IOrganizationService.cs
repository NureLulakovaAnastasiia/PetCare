using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IOrganizationService
    {
        public string AcceptMasterRequest(int requestId);
        public string RejectMasterRequest(int requestId);
        public Task<List<GetRequestDto>> GetNewRequests();

        public Task<string> UpsertMasterSchedule(List<ScheduleDto> scheduleDto, string masterId);
        public Task<string> UpsertMasterBreaks(List<BreakDto> breakDto, string masterId);
    }


}
