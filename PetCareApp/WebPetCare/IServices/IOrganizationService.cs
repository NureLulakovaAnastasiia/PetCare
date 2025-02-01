using PetCareApp.Dtos;
using PetCareApp.Models;

namespace WebPetCare.IServices
{
    public interface IOrganizationService
    {
        public Task<Result<OrganizationInfo>> GetOrganizationInfo();
        public Task<string?> GetCurrentUserRole();
        public Task<string> MakeRequest(int organizationId);
        public Task<string> UpdateOrganizationInfo(OrganizationInfo organizationInfo);
        public Task<Result<OrganizationDetailsDto>> GetOrganizationDetails(int organizationId);
        public Task<List<GetRequestDto>> GetRequests();
        public Task<string> AcceptRequest(int requestId);
        public Task<string> RejectRequest(int requestId);
        public Task<Result<List<GetEmployeeDto>>> GetEmployees(int orgId);

        public Task<string> DismissEmployee(int employeeId);
    }
}
