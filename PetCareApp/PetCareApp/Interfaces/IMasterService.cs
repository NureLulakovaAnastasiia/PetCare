using PetCareApp.Dtos;

namespace PetCareApp.Interfaces
{
    public interface IMasterService
    {
        public Task<string> AddContacts(AddContactsDto contacts);
        public Task<string> UpdateContacts(ContactsDto contacts);

        public Task<string> AddService(AddServiceDto service);

        public Task<string> AddQuestionary(List<AddQuestionDto> questionary, int serviceId);

        public string CheckQuestionary(List<AddQuestionDto> questionary);
    }
}
