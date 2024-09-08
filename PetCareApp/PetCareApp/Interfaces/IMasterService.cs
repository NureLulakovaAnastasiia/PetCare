using PetCareApp.Dtos;
using PetCareApp.Models;

namespace PetCareApp.Interfaces
{
    public interface IMasterService
    {
        public Task<string> AddContacts(AddContactsDto contacts);
        public Task<string> UpdateContacts(ContactsDto contacts);

        public Task<string> AddService(AddServiceDto service);

        public Task<string> UpsertSchedule(List<ScheduleDto> schedule);
        public Task<string> UpsertBreaks(List<BreakDto> breaks);

        public Task<string> AddQuestionary(List<AddQuestionDto> questionary, int serviceId);
        public Task<string> UpdateQuestionary(List<UpdateQuestionDto> questionary);

        public string CheckQuestionary<TQuestion, TAnswer>(List<TQuestion> questionary) 
            where TQuestion : IQuestionDto<TAnswer>
            where TAnswer : IAnswerDto;

        public List<TimeSlot> GetWorkTimeSlots(string masterId);
        public List<TimeSlot> GetFreeTimeSlots(string masterId, int time);

        public Task<string> MakeAppointment(RecordDto recordDto);   
            
    }
}
