using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface IServiceApiService
    {
        public Task<string> AddService(AddServiceDto service);
        public string UpdateService(UpdateServiceDto service);
        public Task<List<GetServiceDto>> GetServices(string? masterId);
        public Task<Result<GetServiceDto>> GetServiceDetails(int serviceId);
        public Task<string> DeleteService(int serviceId);
        public Task<string> ChangeServiceVisibility(int serviceId);
        public Task<string> GetMasterName(int serviceId);

        public Task<Result<List<ReviewDto>>> GetServiceReviews(int serviceId);
    }
}
