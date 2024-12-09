using PetCareApp.Dtos;
using PetCareApp.Models;

namespace WebPetCare.IServices
{
    public interface IUserService
    {
        public Task<Result<GetGeneralMasterDto>> getGeneralMasterData();

        public Task<string> UpdateGeneralMasterData(GetGeneralMasterDto dto);

        public Task<Result<List<BreakDto>>> getMasterBreaks();

        public Task<string> UpdateAppointments(List<GetRecordDto> records);
        public Task<string> UpdateBreaks(List<BreakDto> breaks);
    }
}
