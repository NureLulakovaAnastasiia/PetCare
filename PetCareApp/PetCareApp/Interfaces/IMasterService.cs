using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface IMasterService
    {
        public Task<string> AddContacts(AddContactsDto contacts);
        public Task<string> UpdateContacts(ContactsDto contacts);
        public Task<string> UpsertSchedule(List<ScheduleDto> schedule);
        public Task<string> UpsertBreaks(List<BreakDto> breaks, string? masterId = null);
        public Task<string> DeleteBreaks(List<int> breaksId);
        public Task<string> AddQuestionary(List<AddQuestionDto> questionary, int serviceId);
        public Task<string> UpdateQuestionary(List<UpdateQuestionDto> questionary);
        public Task<List<UpdateQuestionDto>> GetQuestionary(int serviceId);
        public Task<string> DeleteQuestionary(int serviceId);
        public string CheckQuestionary<TQuestion, TAnswer>(List<TQuestion> questionary)
            where TQuestion : IQuestionDto<TAnswer>
            where TAnswer : IAnswerDto;

        public List<TimeSlot> GetWorkTimeSlots(string masterId);
        public List<TimeSlot> GetFreeTimeSlots(int time, int serviceId, string? masterId = null);

        public Task<string> MakeAppointment(RecordDto recordDto);

        public Task<string> UpdateAppointments(List<GetRecordDto> recordDto);

        public int AnalizeQuestionary(List<QuestionDto> questionary, int serviceId);
        public string GetQuestionaryDescription(List<QuestionDto> questionary);

        public Task<string> MakeRequestToOrganization(int organizationId);

        public Task<string> CancelRecord(int recordId, string reason);

        public Task<GetGeneralMasterDto> GetGeneralMasterData();
        public Task<string> UpdateGeneralMasterData(GetGeneralMasterDto masterData);

        public Task<Result<List<GetRecordDto>>> GetRecordsForMonth(int month, int year, string? masterId);
        public Task<Result<List<BreakDto>>> GetMasterBreaks(string? masterId);

        public Task<Result<List<PortfolioDto>>> GetMasterPortfolio(string? masterId);
        public Task<Result<List<int>>> UpsertPortfolio(List<PortfolioDto> portfolio);
        public Task<string> DeletePortfolio(int portfolioId);
        public Task<Result<List<GetServiceDto>>> GetMasterServices(string? masterId);

        public Result<List<GetQuestionDto>> GetQuestionaryForUser(int serviceId);

        public Task<Result<List<ScheduleDto>>> GetMasterSchedule(string? masterId);

        public Result<bool> UpdateMasterSchedule(List<ScheduleDto> schedule);
        public Task<Result<bool>> DeleteSchedule(int scheduleId);
        public Task<Result<bool>> AddMasterSchedule(ScheduleDto schedule);
    } 
}
