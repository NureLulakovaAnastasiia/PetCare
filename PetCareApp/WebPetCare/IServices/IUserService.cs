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
        public Task<string> UpsertBreaks(List<BreakDto> breaks);
        public Task<string> AddRecord(RecordDto record);
        public Task<string> DeleteBreaks(List<int> breaks);
        public Task<string> CancelAppointment(int appointmentId, string reason);

        public Task<Result<List<PortfolioDto>>> GetMasterPortfolio(string? masterId = null);
        public Task<Result<Dictionary<int, string>>> GetMasterServicesNames();
        public Task<Result<List<int>>> UpsertPortfolio(List<PortfolioDto> portfolio);

        public Task<string> DeletePortfolio(int portfolioId); 
    }
}
