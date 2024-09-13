using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IOrganizationService
    {
        public string AcceptMasterRequest(int requestId);
        public string RejectMasterRequest(int requestId);
        public Task<List<GetRequestDto>> GetNewRequests();
    }


}
