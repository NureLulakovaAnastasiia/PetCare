using PetCareApp.Models;

namespace PetCareApp.Dtos
{
    public class QuestionaryAnalisysDto
    {
        public int Time {  get; set; }
        public List<TimeSlot> Slots { get; set; }

        public string Description { get; set; }
    }
}
