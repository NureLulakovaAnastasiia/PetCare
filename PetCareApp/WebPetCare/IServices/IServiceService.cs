using PetCareApp.Dtos;
using PetCareApp.Models;

namespace WebPetCare.IServices
{
    public interface IServiceService
    {
        public Task<Result<GetServiceDto>> getServiceDetails(int serviceId);
        public Task<string> UpdateService(GetServiceDto service);
        public Task<string> AddService(AddServiceDto serviceDto, List<AddQuestionDto> questions);

        public Task<string> DeleteService(int serviceId);
        public Task<string> ChangeServiceVisibility(int serviceId);
        public Task<string> UpdateQuestionary(List<UpdateQuestionDto> questionaryDto);
        public Task<Result<List<UpdateQuestionDto>>> GetQuestionary(int serviceId);
        public Task<string> DeleteQuestionary(int serviceId);
        public Task<Result<List<GetRecordDto>>> getMasterRecordForMonth(DateTime startDate);

        public Task<Result<string>> GetMasterName(int serviceId);

        public Task<string?> GetCurrentUserRole();
    }
}
