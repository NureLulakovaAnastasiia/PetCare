using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IMasterService
    {
        public Task<string> AddContacts(AddContactsDto contacts);
        public Task<string> UpdateContacts(ContactsDto contacts);

        public Task<string> AddService(AddServiceDto service);

        public Task<string> UpsertSchedule(List<ScheduleDto> schedule);

        public Task<string> AddQuestionary(List<AddQuestionDto> questionary, int serviceId);
        public Task<string> UpdateQuestionary(List<UpdateQuestionDto> questionary);

        public string CheckQuestionary<TQuestion, TAnswer>(List<TQuestion> questionary) 
            where TQuestion : IQuestionDto<TAnswer>
            where TAnswer : IAnswerDto;
    }
}
