using PetCareApp.Dtos;
using PetCareApp.Models;

namespace WebPetCare.IServices
{
    public interface IServiceService
    {
        public Task<Result<GetServiceDto>> getServiceDetails(int serviceId);
        public Task<string> UpdateService(GetServiceDto service);
    }
}
