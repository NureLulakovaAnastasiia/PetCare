using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IServiceApiService
    {
        public Task<string> AddService(AddServiceDto service);
        public string UpdateService(UpdateServiceDto service);
        public Task<List<GetServiceDto>> GetServices(string? masterId);
        public GetServiceDto GetServiceDetails(int serviceId);
    }
}
