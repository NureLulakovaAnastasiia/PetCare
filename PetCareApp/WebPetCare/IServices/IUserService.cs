using PetCareApp.Dtos;
using PetCareApp.Models;

namespace WebPetCare.IServices
{
    public interface IUserService
    {
        public Task<Result<GetGeneralMasterDto>> getGeneralMasterData();

        public Task<string> UpdateGeneralMasterData(GetGeneralMasterDto dto);

        public Task<Result<List<BreakDto>>> getMasterBreaks(string? masterId);

        public Task<string> UpdateAppointments(List<GetRecordDto> records);
        public Task<string> UpsertBreaks(List<BreakDto> breaks, string? masterId = null);
        public Task<string> AddRecord(RecordDto record);
        public Task<string> DeleteBreaks(List<int> breaks);
        public Task<string> CancelAppointment(int appointmentId, string reason);

        public Task<Result<List<PortfolioDto>>> GetMasterPortfolio(string? masterId = null);
        public Task<Result<Dictionary<int, string>>> GetMasterServicesNames(string? masterId = null);
        public Task<Result<List<int>>> UpsertPortfolio(List<PortfolioDto> portfolio);

        public Task<string> DeletePortfolio(int portfolioId); 

        public Task<Result<List<GetServiceDto>>> GetMasterServices(string? masterId = null);

        public Task<string?> GetCurrentUserRole();

        public Task<Result<int>> AddPet(PetDto pet);

        public Task<Result<string>> UpdatePets(List<PetDto> pets);

        public Task<string> DeletePet(int petId);

        public Task<Result<MasterDto>> GetMasterData(string masterId);
        
        public Task<Result<List<ReviewDto>>> GetMasterReviews(string masterId);

        public Task<Result<List<GetQuestionDto>>> GetServiceQuestionary(int serviceId);

        public Task<Result<QuestionaryAnalisysDto>> AnalizeQuestionary(List<QuestionDto> questionary, int serviceId);

        public Task<Result<List<RecordInfoDto>>> GetUserRecords();

        public Task<string> CancelRecord(int recordId);

        public Task<string> AddReview(ReviewDto review);
        public Task<string> AddReviewComment(ReviewCommentDto comment);

        public Task<string> DeleteReview(int reviewId);
        public Task<string> DeleteReviewComment(int commentId);
        public Task<Result<List<ScheduleDto>>> GetMasterSchedule(string? masterId);
        public Task<string> UpdateMasterSchedule(List<ScheduleDto> schedules);
        public Task<string> DeleteMasterSchedule(int scheduleId);

        public Task<string> AddMasterSchedule(ScheduleDto schedules);

        public Task<string> UserRoleToMaster();
    }
}
